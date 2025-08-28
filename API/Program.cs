using System.Text.Json.Serialization;
using API.Middleware;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// 1) Services (register everything BEFORE Build)
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors();

// Redis (singleton)
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var connString = builder.Configuration.GetConnectionString("Redis")
        ?? throw new InvalidOperationException("Redis connection string is not configured.");

    var options = ConfigurationOptions.Parse(connString, ignoreUnknown: true);
    options.AbortOnConnectFail = false; // mirrors abortConnect=False

    return ConnectionMultiplexer.Connect(options);
});

builder.Services.AddScoped<ICartService, CartService>();

// 🔐 Identity (as per Sir's transcript)
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<StoreContext>();

// 2) Build
var app = builder.Build();

// 3) Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(opt =>
{
    opt.AllowAnyHeader()
       .AllowAnyMethod()
       .WithOrigins("http://localhost:4200", "https://localhost:4200") // Angular client origin
       .AllowCredentials();                  // ✅ allow cookies
});


// app.UseCors(x => x
//     .AllowAnyHeader()
//     .AllowAnyMethod()
//     .WithOrigins("http://localhost:4200", "https://localhost:4200")
// );

// 🔐 Auth middlewares
app.UseAuthentication();
app.UseAuthorization();

// 4) Endpoints
app.MapControllers();

// Map Identity minimal API endpoints (register, login, etc.)
app.MapGroup("api").MapIdentityApi<AppUser>();

// 5) Migrate + Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Migration/Seeding failed");
        throw;
    }
}

app.Run();
 