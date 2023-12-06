using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularFrontend", builder =>
    {
        builder.WithOrigins("http://140.238.213.255:8088/test-results") // Replace with your Angular frontend URL
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowAngularFrontend");

app.UseHttpsRedirection();

app.MapGet("/runtests", async () =>
{
    var cwd = Directory.GetCurrentDirectory();
    var path = @"C:\Schule\4AHIF\LeoCode\backend\languages";
    
    cwd = $@"{path}\Typescript\PasswordChecker";
    
    var processInfo = new ProcessStartInfo("docker", $"run --rm -v {path}:/usr/src/project -w /usr/src/project florianhagmair06/passwordchecker Typescript PasswordChecker");

    processInfo.CreateNoWindow = true;
    processInfo.UseShellExecute = false;
    processInfo.RedirectStandardOutput = true;
    processInfo.RedirectStandardError = true;



    var proc = new Process
    {
        StartInfo = processInfo,
        EnableRaisingEvents = true
    };

    proc.Start();
    proc.BeginOutputReadLine();
    await proc.WaitForExitAsync();

    var code = proc.ExitCode;
    proc.Dispose();

    var resultsFile = Directory.EnumerateFiles($"{cwd}\\results", "*.json").FirstOrDefault();

    string jsonString = File.ReadAllText(resultsFile);

    JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
    JsonElement rootElement = jsonDocument.RootElement;
    return rootElement;
})
.WithName("RunTests")
.WithOpenApi();

app.Run();