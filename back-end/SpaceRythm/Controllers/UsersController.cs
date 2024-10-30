using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceRythm.Entities;
using SpaceRythm.Data;
//using SpaceRythm.Attributes;
using SpaceRythm.Interfaces;
using SpaceRythm.Models;
using SpaceRythm.Models.User;
using SpaceRythm.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace SpaceRythm.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _userService;
    private readonly MyDbContext _context;
    private readonly IUserStatisticsService _userStatisticsService;

    public UsersController(IUserService userService, IUserStatisticsService userStatisticsService, MyDbContext context, ILogger<UsersController> logger)
    {
        _userService = userService;
        _userStatisticsService = userStatisticsService;
        _context=context;
        _logger = logger;
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
    //[HttpGet("/api/users/{id}")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
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

    // Отримати конкретного user по email
    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        Console.WriteLine($"UserController Received email: {email}");
        var user = await _userService.GetByEmail(email);
    
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    // Отримати avatar
    [HttpGet("avatar")]
    public async Task<IActionResult> GetUserAvatar()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("User ID not found or invalid.");
        }

        var avatarFileName = await _userService.GetUserAvatarNameAsync(userId);
        if (string.IsNullOrEmpty(avatarFileName))
        {
            return NotFound("Avatar not found.");
        }

        // Формуємо повний шлях до файлу
        var avatarPath = Path.Combine("wwwroot", "avatars", avatarFileName);

        if (!System.IO.File.Exists(avatarPath))
        {
            return NotFound("Avatar file not found on server.");
        }

        var avatarBytes = await System.IO.File.ReadAllBytesAsync(avatarPath);
        return File(avatarBytes, "image/jpeg"); 
    }

    // Завантаження профілю зображення

    [Authorize]
    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile avatar)
    {
        _logger.LogInformation("Початок методу UploadAvatar");
       
        if (User.Identity.IsAuthenticated)
        {
            var claims = User.Claims.ToList();
            foreach (var claim in claims)
            {
                _logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
        }
        else
        {
            _logger.LogWarning("User is not authenticated.");
        }

        if (!int.TryParse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value, out int userId))
        {
            return Unauthorized(new { message = "User not authorized" });
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return BadRequest(new { message = "User not found" });

        if (avatar == null || avatar.Length == 0)
            return BadRequest(new { message = "Invalid avatar file" });

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{avatar.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await avatar.CopyToAsync(stream);
        }

        var avatarPath = await _userService.UploadAvatar(user.Id, uniqueFileName);

        return Ok(new { avatarPath });
    }

    // Перевірка, чи поточний user is an admin

    [Authorize]
    [HttpPost("isAdmin")]
    public IActionResult IsAdmin()
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
            return BadRequest(new { message = "User not found" });

        return Ok(user.IsAdmin);
    }

    // Update інформації користувача
    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateUserRequest req)
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
            return BadRequest(new { message = "User not found" });

        var res = await _userService.Update(user.Id.ToString(), req);
        return Ok(res);
    }


    [Authorize]
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
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var user = HttpContext.Items["User"] as User;

        if (user == null)
            return BadRequest(new { message = "User not found" });

        await _userService.Delete(user.Id);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.Delete(id);
        if (!result)
            return NotFound(new { message = "User not found" });

        return Ok(new { message = "User deleted successfully" });
    }

    // Підписка до іншого користувача для отримання оновлень від нього
    [Authorize]
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
    [Authorize]
    [HttpGet("{id}/followers")]
    public async Task<IActionResult> GetFollowers(int id)
    {
        var followers = await _userService.GetFollowers(id);
        return Ok(followers);
    }

    // Створення плейлисту
    [Authorize]
    [HttpPost("create-playlist")]
    public async Task<IActionResult> CreatePlaylist(string name, string description)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return BadRequest(new { message = "User not found" });

        var playlist = await _userService.CreatePlaylist(user.Id, name, description);
        return Ok(playlist);
    }

    [Authorize]
    [HttpGet("{userId}/playlists")]
    public async Task<IActionResult> GetPlaylists(int userId)
    {
        var playlists = await _userService.GetPlaylists(userId);
        if (playlists == null || !playlists.Any())
            return NotFound(new { message = "No playlists found for the user" });

        return Ok(playlists);
    }

    // Метод для додавання треку до плейлиста
    [Authorize]
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
    [Authorize]
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

    [Authorize]
    [HttpGet("{userId}/followers-count")]
    public async Task<IActionResult> GetFollowersCount(int userId)
    {
        var count = await _userStatisticsService.GetFollowersCount(userId);
        return Ok(new { FollowersCount = count });
    }

    [Authorize]
    [HttpGet("{userId}/listenings-count")]
    public async Task<IActionResult> GetListeningsCount(int userId)
    {
        var count = await _userStatisticsService.GetListeningsCount(userId);
        return Ok(new { ListeningsCount = count });
    }

    [Authorize]
    [HttpGet("{userId}/likes-count")]
    public async Task<IActionResult> GetLikesCount(int userId)
    {
        var count = await _userStatisticsService.GetLikesCount(userId);
        return Ok(new { LikesCount = count });
    }

    [Authorize]
    [HttpGet("{userId}/comments-count")]
    public async Task<IActionResult> GetCommentsCount(int userId)
    {
        var count = await _userStatisticsService.GetCommentsCount(userId);
        return Ok(new { CommentsCount = count });
    }
}