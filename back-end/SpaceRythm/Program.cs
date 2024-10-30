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


    // Íàëàøòóâàííÿ àâòåíòèô³êàö³¿ (JWT ³ Cookies)
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

            // Äîäàºìî NameClaimType äëÿ ðîçï³çíàâàííÿ "nameid" ÿê "ClaimTypes.NameIdentifier"
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

    // Íàëàøòóâàííÿ ïîë³òèêè ïàðîë³â âðó÷íó
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

    // Middleware äëÿ ëîãóâàííÿ claims àóòåíòèô³êîâàíîãî êîðèñòóâà÷à
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

// Ensure that Razor Pages are mapped
    app.MapRazorPages();
    app.MapControllers();
}




//var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders(); // Î÷èùåííÿ ïðîâàéäåð³â, ùîá ïî÷àòè ç íóëÿ
//builder.Logging.AddConsole(); // Äîäàâàííÿ ëîãóâàííÿ â êîíñîëü

//Console.WriteLine("UTC Time: " + DateTime.UtcNow);
//Console.WriteLine("Local Time: " + DateTime.Now);


//var loggerFactory = LoggerFactory.Create(builder =>
//{
//    builder.AddConsole(); // Äîäàº ëîãóâàííÿ â êîíñîëü
//    builder.SetMinimumLevel(LogLevel.Debug); // Óñòàíîâëþº ì³í³ìàëüíèé ð³âåíü ëîãóâàííÿ
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

//// Ìåòîä äëÿ êîíô³ãóðàö³¿ ñåðâ³ñ³â
//void ConfigureServices(IServiceCollection services, IConfiguration configuration)
//{
//    // Çàâàíòàæåííÿ íàëàøòóâàíü JWT
//    var jwtSettingsSection = configuration.GetSection("JwtSettings");
//    var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

//    services.Configure<JwtSettings>(jwtSettingsSection);

//    // Ï³äêëþ÷åííÿ äî MySQL
//    var connectionString = configuration.GetConnectionString("DefaultConnection");
//    services.AddDbContext<MyDbContext>(options =>
//        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//    //services.AddIdentity<User, IdentityRole>() // Make sure User inherits from IdentityUser
//    //    .AddEntityFrameworkStores<MyDbContext>()
//    //    .AddDefaultTokenProviders();

//    // Äîäàâàííÿ ñåñ³¿
//    services.AddSession(options =>
//    {
//        options.IdleTimeout = TimeSpan.FromMinutes(30);
//        options.Cookie.HttpOnly = true;
//    });

//    // Íàëàøòóâàííÿ àâòåíòèô³êàö³¿ ÷åðåç JWT
//    services.AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Âèêîðèñòàííÿ êóêè äëÿ àóòåíòèô³êàö³¿
//        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    })
//    .AddCookie()
//    .AddGoogle("Google", options =>
//    {
//        options.ClientId = configuration["Google:ClientId"];
//        options.ClientSecret = configuration["Google:ClientSecret"];

//        // Ïåðåâ³ðêà íà íàÿâí³ñòü ClientId ³ ClientSecret
//        if (string.IsNullOrEmpty(options.ClientId) || string.IsNullOrEmpty(options.ClientSecret))
//        {
//            throw new Exception("Google ClientId or ClientSecret is not configured properly.");
//        }

//        options.Scope.Add("email");
//        options.Scope.Add("profile");
//        options.SaveTokens = true; // Çáåð³ãàòè òîêåíè
//        options.CallbackPath = "/users/google-response";

//        options.Events = new OAuthEvents
//        {
//            OnRedirectToAuthorizationEndpoint = context =>
//            {
//                // Ëîãóâàííÿ ñòàíó ïåðåä ïåðåíàïðàâëåííÿì
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
//                // Ëîãóâàííÿ òîêåíà ï³ñëÿ éîãî ñòâîðåííÿ
//                Console.WriteLine($"Google OAuth ticket created: {context.AccessToken}");
//                return Task.CompletedTask;
//            },
//            OnRemoteFailure = context =>
//            {
//                Console.WriteLine($"OAuth Error: {context.Failure?.Message}");
//                // Ëîãóâàííÿ äëÿ ïîâíî¿ ïîìèëêè
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

//    // Äîäàâàííÿ ñåðâ³ñ³â
//    services.AddScoped<IUserService, UserService>();
//    services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//    services.AddScoped<SignInManager<User>>();

//    // Äîäàâàííÿ ñåðâ³ñ³â äëÿ API êîíòðîëåð³â
//    services.AddControllers();

//    // Äîäàâàííÿ Razor Pages
//    services.AddRazorPages();
//}

//// Ìåòîä äëÿ êîíô³ãóðàö³¿ middleware
//void ConfigureMiddleware(WebApplication app)
//{
//    // ßêùî íå â ðåæèì³ ðîçðîáêè, âèêîðèñòîâóºìî ñïåö³àëüíèé îáðîáíèê ïîìèëîê
//    if (!app.Environment.IsDevelopment())
//    {
//        app.UseExceptionHandler("/Error");
//        app.UseHsts();
//    }

//    // Äîäàâàííÿ âèêîðèñòàííÿ ñåñ³¿
//    app.UseSession();

//    app.UseCookiePolicy(new CookiePolicyOptions
//    {
//        MinimumSameSitePolicy = SameSiteMode.Lax, // àáî None äëÿ êðàùî¿ ñóì³ñíîñò³
//        Secure = CookieSecurePolicy.Always, // îáîâ'ÿçêîâî äëÿ HTTPS
//    });

//    app.UseHttpsRedirection();
//    app.UseRouting();

//    app.UseAuthentication();
//    app.UseAuthorization();

//    // Çàáåçïå÷óºìî ìàðøðóòèçàö³þ äëÿ Razor Pages
//    app.MapRazorPages();

//    // Ìàðøðóòèçàö³ÿ äëÿ API êîíòðîëåð³â
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
//    options.SaveTokens = true; // Çáåðåãòè òîêåíè
//    options.CallbackPath = "/signin-google"; // URL äëÿ ïîâåðíåííÿ ï³ñëÿ àâòîðèçàö³¿
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


