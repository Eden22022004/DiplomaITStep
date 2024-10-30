using SpaceRythm.Data;
using SpaceRythm.Entities;

namespace SpaceRythm.Models.Middlewares;

public class UserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MyDbContext _context;

    public UserMiddleware(RequestDelegate next, MyDbContext context)
    {
        _next = next;
        _context = context;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.Request.Cookies["UserId"]; 

        if (!string.IsNullOrEmpty(userId))
        {
            var user = await GetUserFromDatabase(userId); 
            context.Items["User"] = user; 
        }

        await _next(context); 
    }

    private async Task<SpaceRythm.Entities.User> GetUserFromDatabase(string userId)
    {
        return await _context.Users.FindAsync(userId); 
    }
}