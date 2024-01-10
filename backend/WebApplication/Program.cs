using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace LeoCodeBackend
{
    class Program
    {
        private static Process backendProcess;

        static void Main(string[] args)
        {
            StartBackend();
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
            app.UseCors();
            app.UseHttpsRedirection();

            app.MapPost("/runtests", RunTestsApi)
                .WithName("RunTestsApi")
                .WithOpenApi();

            app.MapPost("/start", StartBackend)
                .WithName("Start")
                .WithOpenApi();

            app.MapPost("/stop", StopBackend)
                .WithName("Stop")
                .WithOpenApi();

            app.MapPost("/runtestssecondbackend", RunTestsSecondBackend)
                .WithName("RunTestsSecondBackend")
                .WithOpenApi();

            app.Run();
        }

        static async Task<string> RunTestsSecondBackend(string language, string ProgramName)
        {
            try
            {
                var apiUrl = $"http://localhost:5055/runtest?language={Uri.EscapeDataString(language)}&ProgramName={Uri.EscapeDataString(ProgramName)}";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonDocument = JsonDocument.Parse(responseContent);
                        var rootElement = jsonDocument.RootElement;
                        var responseObject = new { data = rootElement };
                        return JsonSerializer.Serialize(responseObject);
                    }
                    else
                    {
                        var errorObject = new { error = $"HTTP Error: {response.StatusCode}" };
                        return "error";
                    }
                }
            }
            catch (Exception ex)
            {
                var errorObject = new { error = $"An error occurred: {ex.Message}" };
                return "error";
            }
        }

        static async Task<IActionResult> RunTestsApi(string language, string ProgramName)
        {
            try
            {
                var cwd = Directory.GetCurrentDirectory();

                var path = $@"{cwd}\..\languages";

                cwd = $@"{path}\{language}\{ProgramName}";

                var command = $"run --rm -v {path}:/usr/src/project -w /usr/src/project pwdtest {language} {ProgramName}";
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
                    ResultFileHelperCSharp resultFileHelperCSharp = new ResultFileHelperCSharp();
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

        static void StartBackend()
        {
            try
            {
                string webApiProjectPath = @"../LeoCodeBackend";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "run",
                    WorkingDirectory = webApiProjectPath,
                };

                backendProcess = Process.Start(psi);

                Console.WriteLine("Web API started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting Web API: {ex.Message}");
            }
        }

        static void StopBackend()
        {
            try
            {
                if (backendProcess != null && !backendProcess.HasExited)
                {
                    backendProcess.Kill();
                    backendProcess.WaitForExit(); // Optionally wait for the process to exit
                    Console.WriteLine("Web API process killed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping Web API: {ex.Message}");
            }
        }
    }
}
