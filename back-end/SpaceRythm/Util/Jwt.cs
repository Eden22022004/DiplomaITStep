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
                Issuer = jwtSettings.Issuer, // Додавання Issuer
                Audience = jwtSettings.Audience // Додавання Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Повернення згенерованого токена
            return tokenHandler.WriteToken(token);
        }
    }

//public static class Jwt
//{
//    public static string GenerateToken(JwtSettings jwtSettings, User user)
//    {
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new[]
//            {
//                new Claim("id", user.Id.ToString()), // Перетворення user.Id у рядок
//                new Claim(ClaimTypes.Name, user.Username) // Якщо є Username
//            }),
//            Expires = DateTime.UtcNow.AddDays(jwtSettings.ExpirationInDays),
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
//                                                        SecurityAlgorithms.HmacSha256Signature)
//        };

//        var token = tokenHandler.CreateToken(tokenDescriptor);
//        return tokenHandler.WriteToken(token);
//    }
//}
