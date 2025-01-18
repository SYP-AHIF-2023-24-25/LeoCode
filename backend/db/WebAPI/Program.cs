using Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// 1. Konfiguration der JSON-Optionen und Controller
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 2. CORS-Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularFrontend", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 3. Konfiguration des ConnectionStrings und DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 4. Registrierung von IUnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 5. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 6. Registrierung von PingController
builder.Services.AddScoped<PingController>();

var app = builder.Build();

// 7. Initialisierung und Patch von PingController
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<PingController>();
    await initializer.Patch();
}

// 8. Konfiguration der Middleware
app.UsePathBase("/db");
app.UseCors("AllowAngularFrontend");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
