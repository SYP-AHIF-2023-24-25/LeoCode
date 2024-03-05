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

            app.MapPost("/api/runTSTests", RunTsTests)
                .WithName("RunTsTests")
                .WithOpenApi();

            app.MapPost("/api/runCSharpTests", RunCSharpTests)
                .WithName("RunCSharpTests")
                .WithOpenApi();

            app.MapPost("/api/runJavaTests", RunJavaTests)
                .WithName("RunJavaTests")
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
            var currentDirectory = Directory.GetCurrentDirectory();
            string expressServerFilePath = $@"{currentDirectory}\..\ts-runner";
            Directory.SetCurrentDirectory(expressServerFilePath);
            string command = $"tsc";
            var processInfo = new ProcessStartInfo("npx", command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = new Process { StartInfo = processInfo, EnableRaisingEvents = true })
            {
                process.Start();

                await Task.Run(() =>
                {
                    process.WaitForExit();
                });
            }
            Directory.SetCurrentDirectory(currentDirectory);
        }

        static async void BuildImageExpressServer(){
            try 
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var dockerFilePath = $@"{currentDirectory}\..\ts-runner\Dockerfile";
                var expressServerFilePath = $@"{currentDirectory}\..\ts-runner";
                var command = $"build -f {dockerFilePath} -t ts-runner2 {expressServerFilePath}";
                var processInfo = new ProcessStartInfo("docker", command)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = new Process { StartInfo = processInfo, EnableRaisingEvents = true })
                {
                    process.Start();
                    await process.WaitForExitAsync();
                }
                Directory.SetCurrentDirectory(currentDirectory);
            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task<IActionResult> RunTsTests(string exerciseName, [FromBody] JsonObject arrayOfSnippets)
        {
            string apiUrl = $"http://localhost:8000/api/execute/{exerciseName}";
            HttpResponseMessage response = null;
            Snippets snippets = JsonConvert.DeserializeObject<Snippets>(arrayOfSnippets.ToString());
            //Console.WriteLine(snippets.ArrayOfSnippets[0].FileName);

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string jsonContent = $"{{\"code\":\"{ConcatSnippets(snippets)}\", \"fileName\":\"{snippets.ArrayOfSnippets[0].FileName}\"}}";
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(apiUrl, content);
                    Console.WriteLine(arrayOfSnippets.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
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
        static async Task<IActionResult> RunCSharpTests(string exerciseName, [FromBody] JsonObject arrayOfSnippets)
        {
            /*string apiUrl = $"http://localhost:8001/api/execute/{exerciseName}";
            HttpResponseMessage response = null;
            Snippets snippets = JsonConvert.DeserializeObject<Snippets>(arrayOfSnippets.ToString());

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string jsonContent = $"{{\"snippets\":\"{snippets}\"}}";
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
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
            
            return new OkObjectResult(response.Content.ReadAsStringAsync());*/
            //TODO: Implementierung der C#-Tests
            return null;
        }

        static async Task<IActionResult> RunJavaTests(string exerciseName, [FromBody] JsonObject arrayOfSnippets)
        {
            /*
            string apiUrl = $"http://localhost:8000/api/execute/{exerciseName}";
            HttpResponseMessage response = null;
            Snippets snippets = JsonConvert.DeserializeObject<Snippets>(arrayOfSnippets.ToString());

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string jsonContent = $"{{\"snippets\":\"{snippets}\"}}";
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
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
            return new OkObjectResult(response.Content.ReadAsStringAsync());*/
            //TODO: Impementierung der Java-Tests
            return null;
        }


        static async void InstallingNodeModulesForExpressServer()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var expressServerFilePath = $@"{currentDirectory}\..\ts-runner";
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = expressServerFilePath
            };

            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine("npm install");
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();
            Directory.SetCurrentDirectory(currentDirectory);
        }

        static async void StartExpressServer()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            string expressServerFilePath = $@"{currentDirectory}\..\languages\Typescript";
            Directory.SetCurrentDirectory(expressServerFilePath);
            string command = $"compose up -d";
            var processInfo = new ProcessStartInfo("docker", command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = new Process { StartInfo = processInfo, EnableRaisingEvents = true })
            {
                process.Start();

                await Task.Run(() =>
                {
                    process.WaitForExit();
                });
            }
            Directory.SetCurrentDirectory(currentDirectory);
        }
        static string ConcatSnippets(Snippets snippets)
        {
            try
            {
                string concatedCode = "";
                foreach (Snippet snippetSection in snippets.ArrayOfSnippets)
                {
                    concatedCode += snippetSection.Code;
                }
                return concatedCode;
            }
            catch (System.Text.Json.JsonException ex)
            {
                Console.WriteLine($"Fehler beim Deserialisieren: {ex.Message}");
                return ex.Message;
            }
        }
    }
    public class Snippet
    {
        public string Code { get; set; }
        public bool ReadonlySection { get; set; }
        public string FileName { get; set; }
    }

    public class Snippets
    {
        public Snippet[] ArrayOfSnippets { get; set; }
    }
}