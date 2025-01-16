using AuthDemoApi;

var builder = WebApplication.CreateBuilder(args);

// CORS hinzuf�gen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Erlaubt Anfragen von Angular-Frontend
                  .AllowAnyHeader()                     // Erlaubt beliebige Header
                  .AllowAnyMethod()                     // Erlaubt alle HTTP-Methoden
                  .AllowCredentials();                  // Erlaubt Anfragen mit Anmeldeinformationen (z.B. Cookies)
        });
});

builder.Services.AddLeoAuthentication();
builder.Services.AddBasicLeoAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth();
// Keine Notwendigkeit f�r AddLenientCors, da wir AddCors explizit konfigurieren

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();


// Verwende die konfigurierte CORS-Policy
app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
