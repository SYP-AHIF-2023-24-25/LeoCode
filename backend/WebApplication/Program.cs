using System.Diagnostics;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/runtests", async () =>
{
    var cwd = Directory.GetCurrentDirectory();
    cwd = @"C:\Schule\4AHIF\LeoCode\languages\Typescript\PasswordChecker";
    var processInfo = new ProcessStartInfo("docker", $"run --rm -v {cwd}:/usr/src/project -w /usr/src/project davidpr05/pwdcheck");
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