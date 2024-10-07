using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceRythm.Entities;
using SpaceRythm.Data;
using SpaceRythm.Attributes;
using SpaceRythm.Interfaces;
using SpaceRythm.Models.User;
using Org.BouncyCastle.Ocsp;
using SpaceRythm.DTOs;


namespace SpaceRythm.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // Get users
    [HttpGet]
    //[Route("api/users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    // Admin-досту, щоб get all users
    //[Admin]
    //[HttpGet("/api/[controller]")]
    //public async Task<IActionResult> Get()
    //{
    //    var users = await _userService.GetAll();
    //    return Ok(users);
    //}

    // Отримати конкретного user по id
    //[HttpGet("/api/users/{id}")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetById(id);
        if (user == null)
            return NotFound(new { message = "User not found" });
        return Ok(user);
    }

    // Отримати конкретного user по username
    //[HttpGet("/api/users/by-username/{username}")]
    [HttpGet("by-username/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _userService.GetByUsername(username);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    // Отримати конкретного user по username
    //[HttpGet("/api/users/by-email/{email}")]
    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var user = await _userService.GetByEmail(email);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }


    // Create a new user
    //[HttpPost("/api/[controller]")]
    //[HttpPost]
    //public async Task<IActionResult> Create(CreateUserRequest req)
    //{

    //    try
    //    {
    //        var res = await _userService.Create(req);
    //        return Ok(res);
    //    }
    //    catch (Exception e)
    //    {
    //        return BadRequest(new { message = e.Message });
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest req)
    {
        // Логування отриманих даних
        Console.WriteLine($"Received data: Email={req.Email}, Username={req.Username}, Password={req.Password}");

        // Перевірка, чи дійсно всі дані отримані
        if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
        {
            return BadRequest(new { message = "Email, Username, and Password are required" });
        }

        try
        {
            var res = await _userService.Create(req);
            Console.WriteLine($"User created successfully: {res.Username}");
            return Ok(res);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred: {e.Message}");
            // Повертаємо повідомлення про помилку з деталями для налагодження
            return BadRequest(new { message = $"An error occurred: {e.Message}" });
        }
    }


    // Завантаження профілю зображення
    //[HttpPost("/api/user/upload-avatar")]
    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile avatar)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return BadRequest(new { message = "User not found" });

        // Проверка на наличие файла
        if (avatar == null || avatar.Length == 0)
            return BadRequest(new { message = "Invalid avatar file" });

        // Путь для сохранения аватара
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");

        // Создаем папку, если ее нет
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Генерируем уникальное имя для файла
        var uniqueFileName = $"{Guid.NewGuid()}_{avatar.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Сохраняем файл на сервере
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await avatar.CopyToAsync(stream);
        }

        // Обновляем аватар пользователя в базе данных
        var avatarPath = await _userService.UploadAvatar(user.Id, uniqueFileName);

        return Ok(new { avatarPath });
    }

    // Authenticate a user
    //[HttpPost("/api/[controller]/authenticate")]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest req)
    {
        var res = await _userService.Authenticate(req);

        if (res == null)
            return BadRequest(new { message = "Username or password incorrect" });

        return Ok(res);
    }

    // Перевірка, чи поточний user is an admin
    //[HttpPost("/api/user/isAdmin")]
    [HttpPost("isAdmin")]
    public IActionResult IsAdmin()
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
            return BadRequest(new { message = "User not found" });

        return Ok(user.IsAdmin);
    }

    // Update інформації користувача тільки authorized може)
    //[SpaceRythm.Attributes.Authorize]
    //[HttpPut("/api/user")]
    [SpaceRythm.Attributes.Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserRequest req)
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
            return BadRequest(new { message = "User not found" });

        var res = await _userService.Update(user.Id.ToString(), req);
        return Ok(res);
    }

    // Підписка до іншого користувача для отримання оновлень від нього
    //[HttpPost("/api/users/{id}/follow")]
    [HttpPost("{id}/follow")]
    public async Task<IActionResult> FollowUser(int id)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return BadRequest(new { message = "User not found" });

        await _userService.FollowUser(user.Id, id);
        return Ok(new { message = "Successfully followed the user." });
    }

    // Список підписників користувача
    //[HttpGet("/api/users/{id}/followers")]
    [HttpGet("{id}/followers")]
    public async Task<IActionResult> GetFollowers(int id)
    {
        var followers = await _userService.GetFollowers(id);
        return Ok(followers);
    }


    // Зміна пароля користувача
    //[HttpPost("/api/user/change-password")]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest req)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return BadRequest(new { message = "User not found" });

        await _userService.ChangePassword(user.Id, req);
        return Ok(new { message = "Password successfully changed." });
    }

    // Delete поточного користувача
    //[SpaceRythm.Attributes.Authorize]
    //[HttpDelete("/api/user")]
    [SpaceRythm.Attributes.Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
            return BadRequest(new { message = "User not found" });

        await _userService.Delete(user.Id);
        return Ok();
    }

    // Admin-доступ, щоб видалити користувача за id
    //[Admin]
    //[HttpDelete("/api/user/{id}")]
    [Admin]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        return Ok();
    }
}