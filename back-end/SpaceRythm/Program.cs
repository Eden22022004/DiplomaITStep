using Microsoft.EntityFrameworkCore; // Для DbContext
using SpaceRythm.Data;

var builder = WebApplication.CreateBuilder(args);

// Додаємо рядок підключення до MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

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