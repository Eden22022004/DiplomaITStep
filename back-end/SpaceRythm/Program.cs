using Microsoft.EntityFrameworkCore; // For DbContext
using SpaceRythm.Data;
using SpaceRythm.Util;
using Microsoft.Extensions.Options;
using SpaceRythm.Interfaces;
using SpaceRythm.Services;
using SpaceRythm.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using SpaceRythm.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using SpaceRythm.Pages;
using SpaceRythm.Models.Middlewares;
using System.Security.Claims;



var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); // Clear providers
builder.Logging.AddConsole(); // Add console logging

// Configure services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Middleware to log session start and end times
app.Use(async (context, next) =>
{
    var sessionStartTime = DateTime.UtcNow;
    Console.WriteLine($"Session started at {sessionStartTime} UTC");

    await next.Invoke();

    var sessionEndTime = DateTime.UtcNow;
    Console.WriteLine($"Session ended at {sessionEndTime} UTC");
});

ConfigureMiddleware(app);

app.Run();

// Method to configure services
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var jwtSettingsSection = configuration.GetSection("JwtSettings");
    var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

    services.Configure<JwtSettings>(jwtSettingsSection);

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<MyDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
    });

    //services.AddHttpClient();

    var emailOptions = builder.Configuration.GetSection("EmailSender").Get<EmailHelperOptions>() ?? throw new InvalidOperationException("Email Sender options not found.");

    var requireEmailConfirmed = configuration.GetValue<bool>("RequireConfirmedEmail");


    // Налаштування автентифікації (JWT і Cookies)
    services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

            // Додаємо NameClaimType для розпізнавання "nameid" як "ClaimTypes.NameIdentifier"
            NameClaimType = ClaimTypes.NameIdentifier
        };
    })
    .AddCookie()
    .AddGoogle("Google", options =>
    {
        options.ClientId = configuration["Google:ClientId"];
        options.ClientSecret = configuration["Google:ClientSecret"];
        options.CallbackPath = "/users/google-response";

        options.Events = new OAuthEvents
        {
            OnRedirectToAuthorizationEndpoint = context =>
            {
                var state = context.Properties.Items["state"];
                context.HttpContext.Session.SetString("oauth_state", state);
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            },
            OnCreatingTicket = context =>
            {
                var expectedState = context.HttpContext.Session.GetString("oauth_state");
                if (context.Properties.Items["state"] != expectedState)
                {
                    context.Fail("Invalid state.");
                }
                Console.WriteLine($"Google OAuth ticket created: {context.AccessToken}");
                return Task.CompletedTask;
            },
            OnRemoteFailure = context =>
            {
                Console.WriteLine($"OAuth Error: {context.Failure?.Message}");
                return Task.CompletedTask;
            }
        };
    })
    .AddFacebook(options =>
    {
        options.AppId = configuration["Facebook:AppId"];
        options.AppSecret = configuration["Facebook:AppSecret"];
        options.CallbackPath = "/users/facebook-response";

        options.Events = new OAuthEvents
        {
            OnRedirectToAuthorizationEndpoint = context =>
            {
                var state = context.Properties.Items["state"];
                context.HttpContext.Session.SetString("oauth_state", state);
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            },
            OnCreatingTicket = context =>
            {
                var expectedState = context.HttpContext.Session.GetString("oauth_state");
                if (context.Properties.Items["state"] != expectedState)
                {
                    context.Fail("Invalid state.");
                }
                Console.WriteLine($"Facebook OAuth ticket created: {context.AccessToken}");
                return Task.CompletedTask;
            },
            OnRemoteFailure = context =>
            {
                Console.WriteLine($"OAuth Error: {context.Failure?.Message}");
                return Task.CompletedTask;
            }
        };
    });
    
    //.AddJwtBearer(options =>
    //{
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidateLifetime = true,
    //        ValidateIssuerSigningKey = true,
    //        ValidIssuer = jwtSettings.Issuer,
    //        ValidAudience = jwtSettings.Audience,
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    //    };
    //});

    services.AddAuthorization();

    services.AddEmailHelper(emailOptions);

    // Налаштування політики паролів вручну
    services.Configure<PasswordOptions>(options =>
    {
        options.RequireDigit = true;
        options.RequireNonAlphanumeric = true;
        options.RequiredLength = 10;
    });

    services.AddHttpClient<TestUsersModel>();
    services.AddHttpContextAccessor();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<TokenService>();
    services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    services.AddScoped<IUserStatisticsService, UserStatisticsService>();

    services.AddControllers();
    services.AddRazorPages();
}

// Method to configure middleware
void ConfigureMiddleware(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    //app.UseMiddleware<UserMiddleware>();
   

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.Use(async (context, next) =>
    {
        Console.WriteLine($"Program.cs Incoming Request: {context.Request.Method} {context.Request.Path}");
        await next.Invoke(); // Call the next middleware
        Console.WriteLine($"Program.cs Response Status Code: {context.Response.StatusCode}");
    });

    app.UseSession();

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Lax,
        Secure = CookieSecurePolicy.Always,
    });

    app.UseAuthentication();

    // Middleware для логування claims аутентифікованого користувача
    app.Use(async (context, next) =>
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            foreach (var claim in context.User.Claims)
            {
                Console.WriteLine($"Pr.cs Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
        }
        else
        {
            Console.WriteLine("Pr.cs User is not authenticated.");
        }
        await next.Invoke();
    });

    app.UseAuthorization();
    app.UseMiddleware<JwtMiddleware>();

    app.MapRazorPages();
    app.MapControllers();
}




//var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders(); // Очищення провайдерів, щоб почати з нуля
//builder.Logging.AddConsole(); // Додавання логування в консоль

//Console.WriteLine("UTC Time: " + DateTime.UtcNow);
//Console.WriteLine("Local Time: " + DateTime.Now);


//var loggerFactory = LoggerFactory.Create(builder =>
//{
//    builder.AddConsole(); // Додає логування в консоль
//    builder.SetMinimumLevel(LogLevel.Debug); // Установлює мінімальний рівень логування
//});
//var logger = loggerFactory.CreateLogger<Program>();

//// Configure services
//ConfigureServices(builder.Services, builder.Configuration);

//var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    var sessionStartTime = DateTime.UtcNow;
//    Console.WriteLine($"Session started at {sessionStartTime} UTC");

//    await next.Invoke();

//    var sessionEndTime = DateTime.UtcNow;
//    Console.WriteLine($"Session ended at {sessionEndTime} UTC");
//});

//// Configure HTTP request pipeline
//ConfigureMiddleware(app);

//app.Run();

//// Метод для конфігурації сервісів
//void ConfigureServices(IServiceCollection services, IConfiguration configuration)
//{
//    // Завантаження налаштувань JWT
//    var jwtSettingsSection = configuration.GetSection("JwtSettings");
//    var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

//    services.Configure<JwtSettings>(jwtSettingsSection);

//    // Підключення до MySQL
//    var connectionString = configuration.GetConnectionString("DefaultConnection");
//    services.AddDbContext<MyDbContext>(options =>
//        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//    //services.AddIdentity<User, IdentityRole>() // Make sure User inherits from IdentityUser
//    //    .AddEntityFrameworkStores<MyDbContext>()
//    //    .AddDefaultTokenProviders();

//    // Додавання сесії
//    services.AddSession(options =>
//    {
//        options.IdleTimeout = TimeSpan.FromMinutes(30);
//        options.Cookie.HttpOnly = true;
//    });

//    // Налаштування автентифікації через JWT
//    services.AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Використання куки для аутентифікації
//        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    })
//    .AddCookie()
//    .AddGoogle("Google", options =>
//    {
//        options.ClientId = configuration["Google:ClientId"];
//        options.ClientSecret = configuration["Google:ClientSecret"];

//        // Перевірка на наявність ClientId і ClientSecret
//        if (string.IsNullOrEmpty(options.ClientId) || string.IsNullOrEmpty(options.ClientSecret))
//        {
//            throw new Exception("Google ClientId or ClientSecret is not configured properly.");
//        }

//        options.Scope.Add("email");
//        options.Scope.Add("profile");
//        options.SaveTokens = true; // Зберігати токени
//        options.CallbackPath = "/users/google-response";

//        options.Events = new OAuthEvents
//        {
//            OnRedirectToAuthorizationEndpoint = context =>
//            {
//                // Логування стану перед перенаправленням
//                if (context.Properties.Items.TryGetValue("state", out var state))
//                {
//                    Console.WriteLine($"OAuth state (redirecting): {state}");
//                }
//                else
//                {
//                    Console.WriteLine("OAuth state is missing.");
//                }
//                Console.WriteLine($"OAuth properties: {string.Join(", ", context.Properties.Items.Select(kv => $"{kv.Key}: {kv.Value}"))}");
//                context.Response.Redirect(context.RedirectUri);
//                return Task.CompletedTask;
//            },
//            OnCreatingTicket = context =>
//            {
//                // Логування токена після його створення
//                Console.WriteLine($"Google OAuth ticket created: {context.AccessToken}");
//                return Task.CompletedTask;
//            },
//            OnRemoteFailure = context =>
//            {
//                Console.WriteLine($"OAuth Error: {context.Failure?.Message}");
//                // Логування для повної помилки
//                Console.WriteLine($"Error: {context.Failure}");
//                return Task.CompletedTask;
//            }
//        };
//    })
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings.Issuer,
//            ValidAudience = jwtSettings.Audience,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
//        };
//    });

//    // Додавання сервісів
//    services.AddScoped<IUserService, UserService>();
//    services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//    services.AddScoped<SignInManager<User>>();

//    // Додавання сервісів для API контролерів
//    services.AddControllers();

//    // Додавання Razor Pages
//    services.AddRazorPages();
//}

//// Метод для конфігурації middleware
//void ConfigureMiddleware(WebApplication app)
//{
//    // Якщо не в режимі розробки, використовуємо спеціальний обробник помилок
//    if (!app.Environment.IsDevelopment())
//    {
//        app.UseExceptionHandler("/Error");
//        app.UseHsts();
//    }

//    // Додавання використання сесії
//    app.UseSession();

//    app.UseCookiePolicy(new CookiePolicyOptions
//    {
//        MinimumSameSitePolicy = SameSiteMode.Lax, // або None для кращої сумісності
//        Secure = CookieSecurePolicy.Always, // обов'язково для HTTPS
//    });

//    app.UseHttpsRedirection();
//    app.UseRouting();

//    app.UseAuthentication();
//    app.UseAuthorization();

//    // Забезпечуємо маршрутизацію для Razor Pages
//    app.MapRazorPages();

//    // Маршрутизація для API контролерів
//    app.MapControllers();
//}






//var builder = WebApplication.CreateBuilder(args);

//// Configure JWT settings
//var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>(); // Load JWT settings

//var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
//builder.Services.Configure<JwtSettings>(jwtSettingsSection);

//// Add connection string to MySQL
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<MyDbContext>(options =>
//    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));



//// Configure JWT authentication

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    // Use jwtSettings properties directly
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtSettings.Issuer,
//        ValidAudience = jwtSettings.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
//    };
//})
//.AddGoogle("Google", options =>
//{
//    options.ClientId = builder.Configuration["Google:ClientId"];
//    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
//    options.Scope.Add("email");
//    options.Scope.Add("profile");
//    options.SaveTokens = true; // Зберегти токени
//    options.CallbackPath = "/signin-google"; // URL для повернення після авторизації
//});


//// Add services
//builder.Services.AddScoped<IUserService, UserService>();

//// Add services for API controllers
//builder.Services.AddControllers();

//// Add Razor Pages services
//builder.Services.AddRazorPages();

//var app = builder.Build();

//// Configure HTTP request processing pipeline
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//// Ensure that Razor Pages are mapped
//app.MapRazorPages();

//// Register routes for API controllers
//app.MapControllers();

//app.Run();


