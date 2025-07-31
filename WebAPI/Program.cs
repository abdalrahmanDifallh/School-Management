using Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.AuthenticationService;
using Services.AuthenticationService.RolesService;
using Services.AuthenticationService.UserService;
using Services.Classes;
using Services.Grade;
using Services.LoggerService;
using Services.StudentService;
using Services.Subject;
using Services.TeacherService;
using System.Text;
using WebAPI.Notification;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
// Add Entity Framework
builder.Services.AddDbContext<SchoolManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Customize Identity options if needed
    //options.Password.RequireDigit = true;
    //options.Password.RequireLowercase = true;
    //options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequireUppercase = true;
    //options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<SchoolManagementContext>()
.AddDefaultTokenProviders();

// تكوين JWT Options
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("JwtOptions"));

// **JWT Authentication Configuration - Fixed**
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSection = builder.Configuration.GetSection("JwtOptions");

  
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // Fixed: Use "Secret" instead of "SecretKey" to match appsettings.json
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSection["Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured."))
        ),
        ClockSkew = TimeSpan.FromMinutes(5),
        // Additional security settings
        RequireExpirationTime = true,
        RequireSignedTokens = true
    };

    
});



// باقي الخدمات
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenService>();
builder.Services.AddScoped<IClasseService, ClasseService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));

builder.Services.AddHttpContextAccessor();

builder.Services.AddSignalR();

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure middleware - ORDER IS CRITICAL!
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseDeveloperExceptionPage();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication must come before Authorization and Session!
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapHub<NotificationHub>("/notificationHub");


// CRITICAL: Map API controllers - this was missing!
app.MapControllers();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolManagementContext>();
    try
    {
        context.Database.Migrate();
        Console.WriteLine("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration failed: {ex.Message}");
    }
}

app.SeedData();
app.Run();