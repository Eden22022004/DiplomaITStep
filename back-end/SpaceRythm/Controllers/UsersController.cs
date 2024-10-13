using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceRythm.Entities;
using SpaceRythm.Data;
using SpaceRythm.Attributes;
using SpaceRythm.Interfaces;
using SpaceRythm.Models.User;
using Org.BouncyCastle.Ocsp;
using SpaceRythm.DTOs;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Facebook;


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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetById(id);
        if (user == null)
            return NotFound(new { message = "User not found" });
        return Ok(user);
    }

    // Отримати конкретного user по username
    [HttpGet("by-username/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _userService.GetByUsername(username);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    // Отримати конкретного user по username
    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var user = await _userService.GetByEmail(email);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

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
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest req)
    {
        var res = await _userService.Authenticate(req);

        if (res == null)
            return BadRequest(new { message = "Username or password incorrect" });

        return Ok(res);
    }

    [HttpGet("google")]
    public IActionResult GoogleLogin()
    {
        Console.WriteLine("Google login initiated.");

        // Генеруємо URL для редіректу після успішної автентифікації
        var redirectUrl = Url.Action("GoogleResponse", "Auth", new { }, Request.Scheme);

        // Формуємо параметри для зовнішньої автентифікації
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl
        };

        Console.WriteLine($"OAuth state before redirect: {properties.Items["state"]}");
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse(string redirectUri)
    {
        Console.WriteLine("GoogleResponse invoked with redirectUri: " + redirectUri);

        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (result?.Succeeded != true)
        {
            Console.WriteLine("Google authentication failed.");
            return BadRequest("Authentication failed");
        }

        Console.WriteLine("Google authentication succeeded.");
        Console.WriteLine($"User email: {result.Principal.FindFirst(ClaimTypes.Email)?.Value}");

        // Виклик UserService для автентифікації через Google
        var authenticateResponse = await _userService.AuthenticateWithOAuth(result.Principal);

        Console.WriteLine($"Generated token: {authenticateResponse.JwtToken}");

        // Редірект із JWT токеном
        return Redirect($"{redirectUri}?token={authenticateResponse.JwtToken}");
    }

    [HttpGet("facebook")]
    public IActionResult FacebookLogin()
    {
        Console.WriteLine("Facebook login initiated.");

        var redirectUrl = Url.Action("FacebookResponse", "Users", new { }, Request.Scheme);
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl
        };

        Console.WriteLine($"OAuth state before redirect: {properties.Items["state"]}");
        return Challenge(properties, FacebookDefaults.AuthenticationScheme);
    }

    [HttpGet("facebook-response")]
    public async Task<IActionResult> FacebookResponse(string redirectUri)
    {
        Console.WriteLine("FacebookResponse invoked with redirectUri: " + redirectUri);

        var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

        if (result?.Succeeded != true)
        {
            Console.WriteLine("Facebook authentication failed.");
            return BadRequest("Authentication failed");
        }

        Console.WriteLine("Facebook authentication succeeded.");
        Console.WriteLine($"User email: {result.Principal.FindFirst(ClaimTypes.Email)?.Value}");

        var authenticateResponse = await _userService.AuthenticateWithOAuth(result.Principal); 

        Console.WriteLine($"Generated token: {authenticateResponse.JwtToken}");

        return Redirect($"{redirectUri}?token={authenticateResponse.JwtToken}");
    }

    // Перевірка, чи поточний user is an admin
    [HttpPost("isAdmin")]
    public IActionResult IsAdmin()
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
            return BadRequest(new { message = "User not found" });

        return Ok(user.IsAdmin);
    }

    // Update інформації користувача тільки authorized може)
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
    [HttpGet("{id}/followers")]
    public async Task<IActionResult> GetFollowers(int id)
    {
        var followers = await _userService.GetFollowers(id);
        return Ok(followers);
    }


    // Зміна пароля користувача
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.Delete(id);
        if (!result)
            return NotFound(new { message = "User not found" });

        return Ok(new { message = "User deleted successfully" });
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteFacebookUser([FromBody] FacebookDeletionRequest request)
    {
        // Use the AccessToken from the request
        var userIdString = await _userService.VerifyFacebookRequest(request.AccessToken); // Use AccessToken instead of UserId

        if (string.IsNullOrEmpty(userIdString)) // Check if userId is null or empty
        {
            return BadRequest(new { message = "Invalid request" });
        }

        // Convert userIdString to int
        if (!int.TryParse(userIdString, out int userId)) // This tries to parse the string to an integer
        {
            return BadRequest(new { message = "Invalid user ID format" }); // Handle invalid format
        }

        await _userService.Delete(userId); // Call Delete with the integer userId
        return Ok(new { message = "User data deleted successfully" });
    }

    // Admin-доступ, щоб видалити користувача за id
    //[Admin]
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    await _userService.Delete(id);
    //    return Ok();
    //}
}