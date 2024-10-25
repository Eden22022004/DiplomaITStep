using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceRythm.Entities;
using SpaceRythm.Data;
using SpaceRythm.Attributes;
using SpaceRythm.Interfaces;
using SpaceRythm.Models;
using SpaceRythm.Models.User;
using Org.BouncyCastle.Ocsp;
using SpaceRythm.DTOs;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity.Data;
using ResetPasswordRequest = SpaceRythm.Models.User.ResetPasswordRequest;
using ForgotPasswordRequest = SpaceRythm.Models.User.ForgotPasswordRequest;
using Microsoft.AspNetCore.Authorization;
using SpaceRythm.Services;


namespace SpaceRythm.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService; 
    private readonly EmailHelper _emailHelper;
    private readonly TokenService _tokenService;

    public UsersController(IUserService userService, EmailHelper emailHelper, TokenService tokenService)
    {
        _userService = userService;
        _emailHelper = emailHelper;
        _tokenService = tokenService;
    }

    // Get users
    [HttpGet]
    //[Route("api/users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAll();
        return Ok(users);
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

    private bool SendEmail(string recipientEmail, string subject, string message)
    {
        return _emailHelper.SendEmail(recipientEmail, message, subject);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest req)
    {
        Console.WriteLine($"Received data: Email={req.Email}, Username={req.Username}, Password={req.Password}");

        if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
        {
            return BadRequest(new { message = "Email, Username, and Password are required" });
        }

        try
        {
            // Створюємо користувача
            var res = await _userService.Create(req);
            Console.WriteLine($"!!!User : {res.Id} + {res.Email} + {res.Username}");

            // Перевірка, чи користувач успішно створений
            if (res != null && res.Id > 0)
            {
                Console.WriteLine($"User created successfully: {res.Username}");

                // Логіка для генерації токену підтвердження електронної пошти
                string emailConfirmationToken = await _tokenService.GenerateEmailConfirmationToken(res.Email);
                string confirmationLink = Url.Action("ConfirmEmail", "Users", new { token = emailConfirmationToken, email = res.Email }, Request.Scheme);
                Console.WriteLine($"Controller Create confirmationLink: {confirmationLink}");

                // Відправка листа з посиланням на підтвердження
                //EmailHelper emailHelper = new EmailHelper();

                if (SendEmail(res.Email, "Ви успішно зареєстровані в Space Rythm", confirmationLink))
                {
                    return Ok(new { message = "User created successfully. Please confirm your email." });
                }
                else
                {
                    return BadRequest(new { message = "Error occurred while sending confirmation email." });
                }
            }
            else
            {
                return BadRequest(new { message = "User creation failed." });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred: {e.Message}");
            return BadRequest(new { message = $"An error occurred: {e.Message}" });
        }
    }


    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userService.GetByEmail(email);
        if (user == null) return BadRequest("Invalid email.");

        var result = await _userService.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return Ok("Email confirmed successfully.");
        }
        else
        {
            return BadRequest("Error confirming your email.");
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

    [HttpPost("authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate(AuthenticateRequest req)
    {
        try
        {
            // Call the Authenticate method in the service
            var response = await _userService.Authenticate(req);

            // If the response is null, the username or password is incorrect
            if (response == null)
            {
                // Add model state error for incorrect credentials
                ModelState.AddModelError("", "Username or password incorrect");
                return BadRequest(new { message = "Username or password incorrect" });
            }

            // If authentication succeeded, return the response
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Handle the error for email not confirmed or other issues
            if (ex.Message.Contains("Email not confirmed"))
            {
                ModelState.AddModelError(nameof(req.Email), "Email not confirmed. Please check your inbox and confirm your email.");
                return BadRequest(new { message = ex.Message });
            }

            // General error handling for username/password or other issues
            return BadRequest(new { message = ex.Message });
        }
    }

    //[HttpPost("authenticate")]
    //public async Task<IActionResult> Authenticate(AuthenticateRequest req)
    //{
    //    var res = await _userService.Authenticate(req);

    //    if (res == null)
    //        return BadRequest(new { message = "Username or password incorrect" });

    //    return Ok(res);
    //}

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


    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordRequest model)
    {
        Console.WriteLine("Controller HttpPost(\"forgot-password\")");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userService.GetByEmail(model.Email);
        if (user == null)
        {
            return NotFound(new { message = "User with this email does not exist." });
        }

        var token = await _tokenService.GeneratePasswordResetToken(user.Email);
        var url = Url.Action("ResetPassword", "Users", new { email = user.Email, token }, Request.Scheme);
        Console.WriteLine("---url " + url);
        bool result = _emailHelper.SendEmailResetPassword(user.Email, url);
        if (!result)
        {
            return StatusCode(500, new { message = "Error occurred while sending email." });
        }

        return Ok(new { message = "Password reset link sent to email." });
    }

    [HttpGet("forgot-password")]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        Console.WriteLine("Controller HttpGet(\"forgot-password\")");
        return Ok(new { message = "Enter your email to reset the password." });
    }

    [HttpGet("forgot-password-confirmation")]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
        return Ok(new { message = "Password reset confirmation page" });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Получите пользователя по email
        var user = await _userService.GetByEmail(model.Email);
        if (user == null)
        {
            return NotFound(new { message = "User with this email does not exist." });
        }
        
        // Проверьте токен (возможно, с помощью вашего TokenService)
        var isValidToken = await _tokenService.VerifyPasswordResetToken(model.Email, model.Token);
        if (!isValidToken)
        {
            return BadRequest(new { message = "Invalid or expired token." });
        }

        // Измените пароль
        var isResetSuccessful = await _userService.ResetPasswordAsync(model.Email, model.Token, model.ConfirmPassword);
        if (!isResetSuccessful)
        {
            return StatusCode(500, new { message = "Error occurred while resetting password." });
        }

        return Ok(new { message = "Password has been successfully reset." });
    }

    [HttpGet("reset-password")]
    [AllowAnonymous]
    public IActionResult ResetPassword(string email, string token)
    {
        // Можно добавить проверку токена, если это необходимо

        // Вместо возвращения представления, просто возвращаем JSON
        return Ok(new { Email = email, Token = token });
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