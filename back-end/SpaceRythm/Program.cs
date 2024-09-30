using Microsoft.EntityFrameworkCore; // Для DbContext
using SpaceRythm.Data;
using SpaceRythm.Util;
using Microsoft.Extensions.Options;
using SpaceRythm.Interfaces;
using SpaceRythm.Services;
using SpaceRythm.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Додаємо рядок підключення до MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUserService, UserService>();

// Додаємо сервіси для контролерів API
builder.Services.AddControllers();


var app = builder.Build();

// Налаштування конвеєра обробки HTTP запитів
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Реєструємо маршрути для контролерів
app.MapControllers();

app.Run();