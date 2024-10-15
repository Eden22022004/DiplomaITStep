using Microsoft.EntityFrameworkCore; // ��� DbContext
using SpaceRythm.Data;

var builder = WebApplication.CreateBuilder(args);

// ������ ����� ���������� �� MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ������ ������ ��� ���������� API
builder.Services.AddControllers();

var app = builder.Build();

// ������������ ������� ������� HTTP ������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// �������� �������� ��� ����������
app.MapControllers();

app.Run();