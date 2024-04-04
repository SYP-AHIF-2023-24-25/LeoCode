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

            app.MapPost("/api/runTests", RunTests)
                .WithName("RunTests")
                .WithOpenApi();
            //      return this.httpClient.post(`${this.baseUrl}api/startTsRunner`, null, { headers: headers });


            app.MapPost("/api/startRunner", StartRunner)
                .WithName("StartTsRunner")
                .WithOpenApi();

            app.MapDelete("/api/stopRunner", StopRunner)
                .WithName("StopRunner")
                .WithOpenApi();

            app.Run();
        }

        private static async Task StopRunner(string language)
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                string pathToDockerComposeFile = $@"{currentDirectory}\..\languages\{language}";
                Directory.SetCurrentDirectory(pathToDockerComposeFile);

                var processInfo = new ProcessStartInfo("docker", "compose down")
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

        private static async Task StartRunner(string language) 
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                if (language == "Typescript")
                {
                    var dockerFilePath = $@"{currentDirectory}\..\ts-runner\Dockerfile";
                    var expressServerFilePath = $@"{currentDirectory}\..\ts-runner";
                    await BuildImageServer(dockerFilePath, expressServerFilePath, "ts-runner");
                    await StartContainer("Typescript");
                }
                else if(language == "CSharp")
                {
                    var dockerFilePath = $@"{currentDirectory}\..\csharp-runner\Dockerfile";
                    var expressServerFilePath = $@"{currentDirectory}\..\csharp-runner";
                    await BuildImageServer(dockerFilePath, expressServerFilePath, "csharp-runner");
                    await StartContainer("CSharp");
                } else if(language == "Java") {
                    var dockerFilePath = $@"{currentDirectory}\..\java-runner\Dockerfile";
                    var quarkusServerFilePath = $@"{currentDirectory}\..\java-runner";
                    await BuildImageServer(dockerFilePath, quarkusServerFilePath, "java-runner");
                    await StartContainer("Java");
                }

                Directory.SetCurrentDirectory(currentDirectory); 
            } 
            catch(Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
        }

        private static async Task BuildImageServer(string dockerFilePath, string expressServerFilePath, string imageName){
            try 
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var command = $"build -f {dockerFilePath} -t {imageName} {expressServerFilePath}";
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

        static async Task<IActionResult> RunTests(string exerciseName,string language, [FromBody] JsonObject arrayOfSnippets)
        {
            string apiUrl = "";
            if(language == "Typescript"){
                apiUrl = $"http://localhost:8000/api/execute/{exerciseName}";
            } else if(language == "CSharp"){
                //apiUrl = $"http://localhost:5168/api/execute/{exerciseName}";
                apiUrl = $"http://localhost:8001/api/execute/{exerciseName}";
            } else if(language == "Java"){
                apiUrl = $"http://localhost:8002/api/execute/{exerciseName}";
            }
            
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
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        JsonDocument result = null;
                        JsonElement value = new JsonElement();
                        if (language == "Typescript")
                        {
                            ResultFileHelperTypescript resultFileHelperTypescript = new ResultFileHelperTypescript();
                            result = JsonDocument.Parse(resultFileHelperTypescript.formatData(responseBody));
                            value = result.RootElement;
                        }
                        else if (language == "CSharp")
                        {
                            Console.WriteLine(responseBody);
                            ResultFileHelperCSharp resultFileHelperCSharp = new ResultFileHelperCSharp();
                            result = JsonDocument.Parse(resultFileHelperCSharp.formatXMLToJson(responseBody));
                            value = result.RootElement;
                        }
                        else if (language == "Java")
                        {
                            /*ResultFileHelperTypescript resultFileHelperTypescript = new ResultFileHelperTypescript();
                            result = JsonDocument.Parse(resultFileHelperTypescript.formatData(responseBody));
                            value = result.RootElement;*/
                        }

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

        private static async Task StartContainer(string language)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            string expressServerFilePath = $@"{currentDirectory}\..\languages\{language}";
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