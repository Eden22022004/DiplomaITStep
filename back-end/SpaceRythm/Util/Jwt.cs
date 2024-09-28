using Microsoft.IdentityModel.Tokens;
using SpaceRythm.Entities;
using SpaceRythm.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace SpaceRythm.Util;

public static class Jwt
{
    public static string GenerateToken(JwtSettings jwtSettings, User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()), // Перетворення user.Id у рядок
                new Claim(ClaimTypes.Name, user.Username) // Якщо є Username
            }),
            Expires = DateTime.UtcNow.AddDays(jwtSettings.ExpirationInDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                        SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
