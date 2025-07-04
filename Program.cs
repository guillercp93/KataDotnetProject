using BancoKata.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register TokenService for dependency injection
builder.Services.AddScoped<IService, Service>();

// ...other service registrations...

var app = builder.Build();

app.MapControllers();

app.Run();
