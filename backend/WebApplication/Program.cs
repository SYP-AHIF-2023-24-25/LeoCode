using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200/test-results", "http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();
    });
});
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularFrontend");

app.UseHttpsRedirection();

app.MapPost("/runtests", RunTestsHandler)
    .WithName("RunTests")
    .WithOpenApi();

app.Run();

async Task<IActionResult> RunTestsHandler(string language, string ProgramName)
{
    try
    {
        var cwd = Directory.GetCurrentDirectory();
        var parentDirectory = Directory.GetParent(cwd).FullName;
        var targetDirectory = Path.Combine(parentDirectory, "languages");
        var path = @"C:\Schule\4AHIF\LeoCode\backend\languages\";
        cwd = $@"C:\Schule\4AHIF\LeoCode\backend\languages\{language}\{ProgramName}";

        var command = $"run --rm -v {path}:/usr/src/project -w /usr/src/project pwdchecker {language} {ProgramName}";
        var processInfo = new ProcessStartInfo("docker", command)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using (var proc = new Process { StartInfo = processInfo, EnableRaisingEvents = true })
        {
            proc.Start();
            await proc.WaitForExitAsync();

            var code = proc.ExitCode;

            var resultsFile = Directory.GetFiles($"{cwd}\\results", "*.json").FirstOrDefault();

            if (resultsFile != null)
            {
                string jsonString = await File.ReadAllTextAsync(resultsFile);

                var jsonDocument = JsonDocument.Parse(jsonString);
                var rootElement = jsonDocument.RootElement;

                var responseObject = new { data = rootElement };

                return new OkObjectResult(responseObject);
            }
            else
            {
                var errorObject = new { error = "No results file found." };
                return new BadRequestObjectResult(errorObject);
            }
        }
    }
    catch (Exception ex)
    {
        var errorObject = new { error = $"An error occurred: {ex.Message}" };
        return new BadRequestObjectResult(errorObject);
    }
}