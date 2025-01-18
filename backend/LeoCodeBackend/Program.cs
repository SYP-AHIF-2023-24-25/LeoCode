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
        private static readonly string _logFilePath = @"../logging/logs.txt";
        private static readonly FileLogger _fileLogger = new FileLogger(_logFilePath);

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

            app.MapGet("/api/hello", async () =>
            {
                return new OkObjectResult("Hello World");
            })
                .WithName("Hello")
                .WithOpenApi();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole(); // Hier kannst du auch andere Provider wie z.B. AddDebug() verwenden
            });

            var logger = loggerFactory.CreateLogger<Program>();

            app.UsePathBase("/backend");
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
            if(language == "TypeScript"){
                apiUrl = $"https://localhost:8000/api/execute/{exerciseName}";
            } else if(language == "CSharp"){
                string baseUrl = "";
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                {
                    Console.WriteLine("Production aaaaaaaaaaaaaaaa");
                    //baseUrl = "https://leocode.htl-leonding.ac.at/csharp-runner";
                    baseUrl = "http://leocode-csharp-runner:5168";
                }
                else
                {
                    Console.WriteLine("Development aaaaaaaaaaaaaaaa");
                    baseUrl = "http://localhost:8001";
                }
                apiUrl = $"{baseUrl}/api/execute/{exerciseName}";
                Console.WriteLine(apiUrl);
            } else if(language == "Java"){
                apiUrl = $"http://localhost:8002/api/execute/{exerciseName}";
            }
            
            HttpResponseMessage response = null;
            Snippets snippets = JsonConvert.DeserializeObject<Snippets>(arrayOfSnippets.ToString());
            //Console.WriteLine(snippets.ArrayOfSnippets[0].FileName);
            using HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };
            using (HttpClient httpClient = new HttpClient(handler))
            {
                // Timeout für HttpClient erhöhen
                httpClient.Timeout = TimeSpan.FromMinutes(5);


                try
                {
                    // Überprüfen, ob snippets gültig ist
                    if (snippets == null || snippets.ArrayOfSnippets == null || snippets.ArrayOfSnippets.Length == 0)
                    {
                        throw new ArgumentNullException(nameof(snippets), "The snippets object or its ArrayOfSnippets property is null or empty.");
                    }

                    var body = new
                    {
                        code = ConcatSnippets(snippets), // Assuming ConcatSnippets creates the full code string
                        fileName = snippets.ArrayOfSnippets[0].FileName // Getting the first snippet's file name
                    };

                    // Serialisiere das Body-Objekt in JSON
                    string jsonContent = JsonConvert.SerializeObject(body);

                    // Erstelle den HTTP-Request-Inhalt
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    Console.WriteLine("vor csharp runner");

                    // Debugging des Inhalts
                    string debugContent = await content.ReadAsStringAsync();
                    Console.WriteLine($"Request Body: {debugContent}");
                    Console.WriteLine($"API URL: {apiUrl}");

                    // Sende die POST-Anfrage
                    if (exerciseName == null)
                        Console.WriteLine("exerciseName is null");

                    if (language == null)
                        Console.WriteLine("language is null");

                    if (arrayOfSnippets == null)
                        Console.WriteLine("arrayOfSnippets is null");

                    response = await httpClient.PostAsync(apiUrl, content);
                    Console.WriteLine("nach csharp runner");

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("in if");
                        _fileLogger.Log($"SUCCESS: Response from {language}-runner was successful");

                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Response Body: {responseBody}");

                        JsonDocument result = null;
                        JsonElement value = new JsonElement();

                        // Sprachspezifische Verarbeitung
                        if (language == "TypeScript")
                        {
                            var resultFileHelperTypescript = new ResultFileHelperTypescript();
                            result = JsonDocument.Parse(resultFileHelperTypescript.formatData(responseBody));
                            value = result.RootElement;
                        }
                        else if (language == "CSharp")
                        {
                            var resultFileHelperCSharp = new ResultFileHelperCSharp();
                            result = JsonDocument.Parse(resultFileHelperCSharp.formatXMLToJson(responseBody));
                            value = result.RootElement;
                        }
                        else if (language == "Java")
                        {
                            // Falls später erforderlich, hier Java-spezifische Verarbeitung hinzufügen
                        }

                        return new OkObjectResult(value);
                    }
                    else
                    {
                        // Fehler-Logging für HTTP-Antwort
                        Console.WriteLine("Request failed with status code " + response.StatusCode);
                        _fileLogger.Log($"ERROR: Response from {language}-runner wasn't successful. Status Code: {response.StatusCode}");
                    }
                }
                catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Timeout Error: The request took too long to complete.");
                    _fileLogger.Log($"ERROR: Timeout occurred when calling {language}-runner. Message: {ex.Message}");
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine("Null Reference Error: " + ex.Message);
                    _fileLogger.Log($"ERROR: Null reference encountered in {language}-runner. Message: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                    _fileLogger.Log($"ERROR: An unexpected error occurred in {language}-runner. Message: {ex.Message}");
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