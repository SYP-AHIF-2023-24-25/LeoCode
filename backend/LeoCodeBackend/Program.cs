using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace LeoCodeBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularFrontend", builder =>
                {
                    builder.WithOrigins("http://localhost:4200/test-results", "http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            var app = builder.Build();

            app.UseCors("AllowAngularFrontend");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapPost("/runtest", RunTests)
                .WithName("RunTest")
                .WithOpenApi();

            app.MapPost("/concatsnippets", ConcatSnippets)
                .WithName("ConcatSnippets")
                .WithOpenApi();

            CompileExpressServerAsync();
            InstallingNodeModulesForExpressServer();
            BuildImageExpressServer();
            Thread.Sleep(5000);
            StartExpressServer();

            app.Run();
        }

        private static async Task CompileExpressServerAsync()
        {
            var cwd = Directory.GetCurrentDirectory();
            string expressServerFilePath = $@"{cwd}\..\Express-Server";
            Directory.SetCurrentDirectory(expressServerFilePath);
            string command = $"tsc";
            var processInfo = new ProcessStartInfo("npx", command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var proc = new Process { StartInfo = processInfo, EnableRaisingEvents = true })
            {
                proc.Start();

                await Task.Run(() =>
                {
                    proc.WaitForExit();
                });
            }
            Directory.SetCurrentDirectory(cwd);
        }

        static async void BuildImageExpressServer(){
            try 
            {
                var cwd = Directory.GetCurrentDirectory();
                var dockerFilePath = $@"{cwd}\..\Express-Server\Dockerfile";
                var projectBuildPath = $@"{cwd}\..\Express-Server";
                var command = $"build -f {dockerFilePath} -t ts-runner {projectBuildPath}";
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
                
                    if (code == 0)
                    {
                        Console.WriteLine("Image builed successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Image builed not successfully.");
                    }
                }
                Directory.SetCurrentDirectory(cwd);
            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task<IActionResult> RunTests(string exerciseId, [FromBody] JsonObject snippetSection)
        {
            string apiUrl = $"http://localhost:8000/api/execute/{exerciseId}";
            HttpResponseMessage response = null;
            Snippet snippet = JsonConvert.DeserializeObject<Snippet>(snippetSection.ToString());
            string code = ConcatSnippets(snippet);

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string jsonContent = $"{{\"code\":\"{code}\",\"exerciseId\":\"{exerciseId}\"}}";
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var jsonDocument = JsonDocument.Parse(responseBody);
                        ResultFileHelperTypescript resultFileHelperTypescript = new ResultFileHelperTypescript();
                        var result = JsonDocument.Parse(resultFileHelperTypescript.formatData(responseBody));
                        var value = result.RootElement;
                        return new OkObjectResult(value);
                    }
                    else
                    {
                        Console.WriteLine($"Request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            return new OkObjectResult(response.Content.ReadAsStringAsync());
        }

        static async void InstallingNodeModulesForExpressServer()
        {
            var cwd = Directory.GetCurrentDirectory();
            var path = $@"{cwd}\..\Express-Server";
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = path
            };

            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine("npm install");
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();
            Directory.SetCurrentDirectory(cwd);
        }

        static async void StartExpressServer()
        {
            var cwd = Directory.GetCurrentDirectory();
            string expressServerFilePath = $@"{cwd}\..\languages\Typescript";
            Directory.SetCurrentDirectory(expressServerFilePath);
            string command = $"compose up -d";
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

                await Task.Run(() =>
                {
                    proc.WaitForExit();
                });
            }
            Directory.SetCurrentDirectory(cwd);
        }
        static string ConcatSnippets(Snippet snippet)
        {
            try
            {
                string concatedCode = "";
                foreach (SnippetSection snippetSection in snippet.SnippetSection)
                {
                    concatedCode += snippetSection.Code;
                }
                return concatedCode;
            }
            catch (System.Text.Json.JsonException ex)
            {
                Console.WriteLine($"Fehler beim Deserialisieren: {ex.Message}");
                return null;
            }
        }
    }
    public class SnippetSection
    {
        public string Code { get; set; }
        public bool ReadonlySection { get; set; }
    }

    public class Snippet
    {
        public SnippetSection[] SnippetSection { get; set; }
    }
}
