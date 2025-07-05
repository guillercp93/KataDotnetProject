using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Bank.Models;
using Bank.Services;
using Bank.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add HTTP Client
var baseUrl = builder.Configuration["BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured in appsettings.json");
Console.WriteLine($"Configuring HttpClient with BaseUrl: {baseUrl}");

// Register the HTTP client with a name
builder.Services.AddHttpClient("ApiClient", client => {
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
    
    Console.WriteLine($"HttpClient configured with BaseAddress: {client.BaseAddress}");
});

// Register RequestClient
builder.Services.AddScoped<IRequestClient, RequestClient>();

// Register Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IDataCreditService, DataCreditService>();

// Configure services
builder.Services.Configure<TokenServiceSettings>(
    builder.Configuration.GetSection(TokenServiceSettings.SectionName));

builder.Services.Configure<ServiceSettings>(
    builder.Configuration.GetSection(ServiceSettings.SectionName));

// Add NSwag
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger document
builder.Services.AddOpenApiDocument(document =>
{
    document.PostProcess = d =>
    {
        d.Info = new NSwag.OpenApiInfo
        {
            Version = "v1",
            Title = "Bank API",
            Description = "A simple banking API",
            Contact = new NSwag.OpenApiContact
            {
                Name = "Bank API Support",
                Email = "support@bankapi.com"
            }
        };
    };
    document.DocumentName = "v1";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable the Swagger UI and OpenAPI document
    app.UseOpenApi();
    app.UseSwaggerUi(settings =>
    {
        settings.DocumentTitle = "Bank API";
        settings.Path = "/swagger";
        settings.DocumentPath = "/swagger/v1/swagger.json";
        settings.DocExpansion = "list";
    });
}

// Redirect root URL to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
