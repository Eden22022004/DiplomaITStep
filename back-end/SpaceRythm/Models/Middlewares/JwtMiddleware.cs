using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpaceRythm.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpaceRythm.Models.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtSettings _jwtSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
    {
        _next = next;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachUserToContext(context, token, userService);

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token, IUserService userService)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            var userIdString = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

            if (int.TryParse(userIdString, out var userId))
            {
                var usr = await userService.GetById(userId);
                if (usr == null)
                {
                    Console.WriteLine($"User with ID {userId} not found.");
                }
                else
                {
                    context.Items["User"] = usr;
                    Console.WriteLine($"User {usr.Username} assigned to context.");
                }
            }
            else
            {
                Console.WriteLine($"Invalid user ID: {userIdString}");
            }
        }
        catch (SecurityTokenExpiredException)
        {
            Console.WriteLine("Token has expired.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}