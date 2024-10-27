using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceRythm.Entities;
using SpaceRythm.Data;
using SpaceRythm.Attributes;
using SpaceRythm.Interfaces;
using SpaceRythm.Models;
using SpaceRythm.Models.User;
using SpaceRythm.Services;


namespace SpaceRythm.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserStatisticsService _userStatisticsService;

    public UsersController(IUserService userService, IUserStatisticsService userStatisticsService)
    {
        _userService = userService;
        _userStatisticsService = userStatisticsService;
    }

    // Get users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _userService.GetAll();
        return Ok(users);
        //await _userService.GetAll();
    }

    // Отримати конкретного user по id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
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
        Console.WriteLine($"UserController Received email: {email}");
        var user = await _userService.GetByEmail(email);
    
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
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

    // Створення плейлисту
    [HttpPost("create-playlist")]
    public async Task<IActionResult> CreatePlaylist(string name, string description)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return BadRequest(new { message = "User not found" });

        var playlist = await _userService.CreatePlaylist(user.Id, name, description);
        return Ok(playlist);
    }

    [HttpGet("{userId}/playlists")]
    public async Task<IActionResult> GetPlaylists(int userId)
    {
        var playlists = await _userService.GetPlaylists(userId);
        if (playlists == null || !playlists.Any())
            return NotFound(new { message = "No playlists found for the user" });

        return Ok(playlists);
    }

    // Метод для додавання треку до плейлиста
    [HttpPost("{playlistId}/tracks/{trackId}")]
    public async Task<IActionResult> AddTrackToPlaylist(int playlistId, int trackId)
    {
        try
        {
            await _userService.AddTrackToPlaylist(playlistId, trackId);
            return Ok(new { message = "Track successfully added to the playlist" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Метод для видалення треку з плейлиста
    [HttpDelete("{playlistId}/tracks/{trackId}")]
    public async Task<IActionResult> RemoveTrackFromPlaylist(int playlistId, int trackId)
    {
        try
        {
            await _userService.RemoveTrackFromPlaylist(playlistId, trackId);
            return Ok(new { message = "Track successfully removed from the playlist" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{userId}/followers-count")]
    public async Task<IActionResult> GetFollowersCount(int userId)
    {
        var count = await _userStatisticsService.GetFollowersCount(userId);
        return Ok(new { FollowersCount = count });
    }

    [HttpGet("{userId}/listenings-count")]
    public async Task<IActionResult> GetListeningsCount(int userId)
    {
        var count = await _userStatisticsService.GetListeningsCount(userId);
        return Ok(new { ListeningsCount = count });
    }

    [HttpGet("{userId}/likes-count")]
    public async Task<IActionResult> GetLikesCount(int userId)
    {
        var count = await _userStatisticsService.GetLikesCount(userId);
        return Ok(new { LikesCount = count });
    }

    [HttpGet("{userId}/comments-count")]
    public async Task<IActionResult> GetCommentsCount(int userId)
    {
        var count = await _userStatisticsService.GetCommentsCount(userId);
        return Ok(new { CommentsCount = count });
    }
}