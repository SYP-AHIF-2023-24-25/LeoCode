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

namespace LeoCodeBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            InstallingNodeModulesForExpressServer();
            InstallingNodeModulesForProjectTemplate("Typescript", "PasswordChecker");
            BuildImage("typescript");
            StartExpressServer();
            
            int pid = GetProcessIdByPort(3000);

            StopExpressServer();
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

            app.MapPost("/runtest", runTests)
                .WithName("RunTests")
                .WithOpenApi();

            app.Run();
        }

        static async Task<IActionResult> runTests(string code,string language, string ProgramName){
            string apiUrl = "http://localhost:3000/runtests";
            HttpResponseMessage response = null;

            // Create an instance of HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                
                try
                {
                    // Define the content to be sent in the POST request (replace with your actual content)
                    string jsonContent = $"{{\"code\":\"{code}\",\"language\":\"{language}\",\"programName\":\"{ProgramName}\"}}";
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send a POST request
                    response = await httpClient.PostAsync(apiUrl, content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and output the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var jsonDocument = JsonDocument.Parse(responseBody);
                        ResultFileHelperTypescript resultFileHelperTypescript = new ResultFileHelperTypescript();
                        var result = JsonDocument.Parse(resultFileHelperTypescript.formatData(responseBody));
                        Console.WriteLine(result);
                        Console.WriteLine("=======================================");
                        var value = result.RootElement;
                        Console.WriteLine(value);
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



        static async void InstallingNodeModulesForProjectTemplate(string language, string projectName) {
            var cwd = Directory.GetCurrentDirectory();

            var path = $@"{cwd}\..\languages\{language}\{projectName}";

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
        }

        static async void BuildImage(string language) 
        {
            try 
            {
                var cwd = Directory.GetCurrentDirectory();
                var dockerFilePath = $@"{cwd}\..\languages\Dockerfile.{language}";
                var projectBuildPath = $@"{cwd}\..\languages";
                Console.WriteLine(dockerFilePath);
                Console.WriteLine(projectBuildPath);
                var command = $"build -f {dockerFilePath} -t passwordchecker {projectBuildPath}";
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
                        //Console.WriteLine($"Image builed not successfully. Exit Code: {backendProcess.ExitCode}");
                    }
                }

            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async void InstallingNodeModulesForExpressServer()
        {
            var cwd = Directory.GetCurrentDirectory();

            var path = $@"{cwd}\..\Express-Server";
            Console.WriteLine(path);
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
        }

        static async void StartExpressServer()
        {
            var cwd = Directory.GetCurrentDirectory();
            string expressServerFilePath = $@"{cwd}/../Express-Server/src/app.js";
            var processInfo = new ProcessStartInfo("node", expressServerFilePath)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var proc = new Process { StartInfo = processInfo, EnableRaisingEvents = true })
            {
                proc.Start();
                //await proc.WaitForExitAsync();
            }
        }

        static async void StopExpressServer()
        {
            //TODO: Stop Express Server
        }

        static int GetProcessIdByPort(int portNumber)
        {
            int processId = -1;

            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "tasklist",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo, EnableRaisingEvents = true })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        // Überprüfen, ob die Zeile den Port enthält
                        if (line.Contains($":{portNumber}"))
                        {
                            // Die PID sollte in den ersten Teilen der Zeile sein
                            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (int.TryParse(parts[1], out processId))
                            {
                                break; // Nur die erste gefundene PID zurückgeben
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Abrufen der PID: {ex.Message}");
            }

            return processId;
        }
    }
}
