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

            /*app.MapPost("/runtest", RunTests)
                .WithName("RunTests")
                .WithOpenApi();*/

            app.MapPost("/runtest", RunTests)
                .WithName("RunTest")
                .WithOpenApi();

            InstallingNodeModulesForExpressServer();
            BuildImageExpressServer();
            StartExpressServer();

            app.Run();
        }

        /*static async Task<IActionResult> RunTests(string code,string language, string ProgramName){
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
        }*/

        static async void BuildImageExpressServer(){
            try 
            {
                var cwd = Directory.GetCurrentDirectory();
                var dockerFilePath = $@"{cwd}\..\Express-Server\Dockerfile";
                var projectBuildPath = $@"{cwd}\..\Express-Server";
                Console.WriteLine(dockerFilePath);
                Console.WriteLine(projectBuildPath);
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
                        //Console.WriteLine($"Image builed not successfully. Exit Code: {backendProcess.ExitCode}");
                    }
                }

            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task<IActionResult> RunTests(string exerciseId)
        {
            string apiUrl = $"http://localhost:8000/api/execute/{exerciseId}";
            HttpResponseMessage response = null;

            // Create an instance of HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Define the content to be sent in the POST request (replace with your actual content)
                    //string jsonContent = $"{{\"code\":\"{code}\",\"language\":\"{language}\",\"programName\":\"{ProgramName}\"}}";
                    //HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send a POST request
                    response = await httpClient.PostAsync(apiUrl, null);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and output the response content as a string
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
            Console.WriteLine($"{cwd}");
            string expressServerFilePath = $@"{cwd}\..\languages\Typescript\docker-compose.yml";
            Console.WriteLine(expressServerFilePath);
            string command = $"compose -f {expressServerFilePath} up -d";
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
        }
    }
}
