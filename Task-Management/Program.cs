using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using Task_Management.Middlewares;
using TaskManagement.API.Seeding;
using TaskManagement.BusinessLogic.Interfaces;
using TaskManagement.BusinessLogic.Mapping;
using TaskManagement.BusinessLogic.Services;
using TaskManagement.Domain.Entities.Identity;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Auth;
using TaskManagement.Infrastructure.BackgroundServices;
using TaskManagement.Infrastructure.JWT;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOi..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("TaskManagement.Infrastructure");
            sqlOptions.CommandTimeout(180);
            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
        }
    ));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings!.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Key)
        ),
        RoleClaimType = ClaimTypes.Role
    };

});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "TaskApp_";
});

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<GeneralProfile>());

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<TaskProcessingWorker>(); 

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IAppTaskService, AppTaskService>();
builder.Services.AddScoped<SeedingDefaultRoles>();
builder.Services.AddScoped<SeedingDefaultUsers>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<SeedingDefaultRoles>();
    var userSeeder = scope.ServiceProvider.GetRequiredService<SeedingDefaultUsers>();

    await roleSeeder.SeedAsync();
    await userSeeder.SeedBasicUserAsync();
    await userSeeder.SeedSuperAdminUserAsync();
}

app.Run();