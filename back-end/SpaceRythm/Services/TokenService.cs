using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpaceRythm.Data;
using SpaceRythm.Entities;
using SpaceRythm.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpaceRythm.Services;

public class TokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly MyDbContext _context;
    public TokenService(MyDbContext context, IOptions<JwtSettings> jwtOptions) 
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
    }
    public string GenerateToken(JwtSettings jwtSettings, User user)
    {
        // Перевірка наявності та коректності значень у JwtSettings
        if (string.IsNullOrWhiteSpace(jwtSettings.Key))
            throw new ArgumentException("Secret key is missing in JWT settings.");

        if (jwtSettings.ExpiresInMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(jwtSettings.ExpiresInMinutes), "ExpirationInDays must be greater than zero.");

        if (string.IsNullOrWhiteSpace(jwtSettings.Issuer))
            throw new ArgumentException("Issuer is missing in JWT settings.");

        if (string.IsNullOrWhiteSpace(jwtSettings.Audience))
            throw new ArgumentException("Audience is missing in JWT settings.");

        // Генерація ключа
        var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

        // Опис токена
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("id", user.Id.ToString()), // Перетворення user.Id у рядок
                    new Claim(ClaimTypes.Name, user.Username) // Username
                }),
            Expires = DateTime.UtcNow.AddDays(jwtSettings.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings.Issuer, 
            Audience = jwtSettings.Audience 
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Повернення згенерованого токена
        return tokenHandler.WriteToken(token);
    }
    public async Task<string> GenerateEmailConfirmationToken(string email)
    {

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new ArgumentException("User with the given email does not exist");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GeneratePasswordResetToken(string email)
    {

        Console.WriteLine("UserService GeneratePasswordResetToken");
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new ArgumentException("User with the given email does not exist");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes), // Set your token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> VerifyPasswordResetToken(string email, string token)
    {
        Console.WriteLine($"UserService VerifyPasswordResetToken");
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
