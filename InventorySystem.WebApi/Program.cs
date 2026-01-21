using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Services;
using InventorySystem.Infrastructure.Data;
using InventorySystem.Infrastructure.Repositories;
using InventorySystem.Infrastructure.Services;
using InventorySystem.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT token vào đây"
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
            new string[] { }
        }
    });
});

// Database Configuration (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register unit of work
// Note: Repositories are created by UnitOfWork directly (not through DI)
// This ensures all repositories use the same DbContext instance as UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register application services
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Register Identity services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<InventorySystem.Application.Interfaces.IAuthenticationService, InventorySystem.Application.Services.AuthenticationService>();
builder.Services.AddScoped<InventorySystem.Application.Interfaces.IAuthorizationService, AuthorizationService>();

#region Authentication Configuration

// Configure Authentication (Custom JWT Handler)
builder.Services
    .AddAuthentication("JwtAuthentication")
    .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("JwtAuthentication", options => { });

#endregion

#region Authorization Configuration

// Configure Authorization with Permission policies
builder.Services.AddAuthorization(options =>
{
    // 1. DefaultPolicy cho [Authorize] no parameters
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // 2. Custom policies cho [Authorize(Policy = "...")]
    //options.AddPolicy("CreateWarehouse", policy =>
    //    policy.Requirements.Add(new PermissionRequirement("Warehouse.Create")));

    //options.AddPolicy("UpdateWarehouse", policy =>
    //    policy.Requirements.Add(new PermissionRequirement("Warehouse.Update")));

    //options.AddPolicy("DeleteWarehouse", policy =>
    //    policy.Requirements.Add(new PermissionRequirement("Warehouse.Delete")));

    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("SuperAdmin"));

    options.AddPolicy("ManagerOrAbove", policy =>
        policy.RequireRole("SuperAdmin", "Manager"));
});

#endregion

//// Register Permission Authorization Handler
//builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

//// Add policy provider for dynamic permission policies
//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventorySystem API v1");
    c.RoutePrefix = "swagger"; // swagger UI at /swagger
});

app.UseCors("AllowAll");

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Ensure database is created (or migrated)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
