using System.Text.Json.Serialization;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using API.Middleware;
using StackExchange.Redis;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Services (ALL registrations must be BEFORE Build)
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors();

// ✅ Register Redis as a singleton BEFORE Build()
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var connString = builder.Configuration.GetConnectionString("Redis")
        ?? throw new InvalidOperationException("Redis connection string is not configured.");

    var options = ConfigurationOptions.Parse(connString, ignoreUnknown: true);
    options.AbortOnConnectFail = false; // mirrors abortConnect=False

    return ConnectionMultiplexer.Connect(options); // return implementation
});
builder.Services.AddScoped<ICartService, CartService>();

// 2) Build
var app = builder.Build();

// 3) Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins(
        "http://localhost:4200",
        "https://localhost:4200"
    )
);

// 4) Endpoints
app.MapControllers();

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
