using Microsoft.OpenApi.Models;
using BancoKata.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register TokenService for dependency injection
builder.Services.AddScoped<IService, Service>();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banco Kata API",
        Version = "v1",
        Description = "API for Banco Kata application",
        Contact = new OpenApiContact
        {
            Name = "Banco Kata Team",
            Email = "support@bancokata.com"
        }
    });
    
    // Enable annotations for Swagger
    c.EnableAnnotations();
    
    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banco Kata API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
    });
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
