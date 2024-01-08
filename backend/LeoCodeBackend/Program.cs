using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace SecondLeoCodeBackend 
{
    class Program
    {
        private static Process backendProcess;

        static void Main(string[] args)
        {
            InstallingNodeModules("Typescript", "PasswordChecker");
            BuildImage("typescript");
            
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
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
            
            app.MapPost("/runtest", RunTests)
                .WithName("RunTests")
                .WithOpenApi();
            
            app.MapPost("/buildimage", BuildImage)
                .WithName("BuildImage")
                .WithOpenApi();

            app.MapPost("/replacecode", ReplaceCode)
                .WithName("ReplaceCode")
                .WithOpenApi();

            app.Run();
        }

        static async void InstallingNodeModules(string language, string projectName) {
            var cwd = Directory.GetCurrentDirectory();

            //var path = $@"{cwd}\..\languages\{language}\{projectName}";
            var path = @"C:\Schule\4AHIF\LeoCode\backend\languages\Typescript\PasswordChecker";

            /*Console.WriteLine(path);
            //var command = $"install --prefix {path}";
            var processInfo = new ProcessStartInfo
            {
                FileName = "npm",
                Arguments = "install",
                WorkingDirectory = path,
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
                if (code == 0) {
                    Console.WriteLine("Erfolgreich node_modules heruntergeladen");
                } else {
                    Console.WriteLine("Fehler beim herunterladen der node_modules");
                }
            }*/
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

            // Send npm install command to the command prompt
            process.StandardInput.WriteLine("npm install");
            process.StandardInput.Flush();
            process.StandardInput.Close();

            // Capture and print the output
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();
        }

        static void ReplaceCode(string code)
        {
            /*
            var cwd = Directory.GetCurrentDirectory();
            string templateFilePath = $"{cwd}/../languages/Typescript/PasswordChecker/src/passwordChecker.ts";
            string[] userCode = ["console.log('Hello World'); return true;", "let name: string = 'Florian'; return false;", "return true;"];

            string templateCode = File.ReadAllText(templateFilePath);
            string[] functionNames = getMethodeNames(templateCode);
            for (int i = 0; i < userCode.Length; i++)
            {
                // Funktion zum Ersetzen des Benutzercode für eine bestimmte Funktion
                string marker = $"// TODO CODE FÜR {functionNames[i]} HIER EINFÜGEN";
                string newCode = $"{marker}\n{userCode[i]}";

                templateCode = templateCode.Replace($"{marker}", newCode);
                File.WriteAllText(templateFilePath, templateCode);
            }*/
            var cwd = Directory.GetCurrentDirectory();
            string templateFilePath = $"{cwd}/../languages/Typescript/PasswordChecker/src/passwordChecker.ts";
            string templateCode = File.ReadAllText(templateFilePath);
            templateCode = code;
            File.WriteAllText(templateFilePath, templateCode);

        }
        /*
        static string[] getMethodeNames(string code)
        {
            string[] functionNames = [];
            string[] lines = code.Split("\n");
            foreach (var line in lines)
            {
                if (line.Contains("function"))
                {
                    string functionName = line.Split("function")[1].Split("(")[0].Trim();
                    functionNames.Append(functionName);
                }
            }
            return functionNames;
        }
        */
        static async void BuildImage(string language) 
        {
            try 
            {
                var cwd = Directory.GetCurrentDirectory();
                var dockerFilePath = $@"{cwd}\..\languages\Dockerfile.{language}";
                var projectBuildPath = $@"{cwd}\..\languages";
                Console.WriteLine(dockerFilePath);
                Console.WriteLine(projectBuildPath);
                var command = $"build -f {dockerFilePath} -t pwdtest {projectBuildPath}";
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
                        Console.WriteLine($"Image builed not successfully. Exit Code: {backendProcess.ExitCode}");
                    }
                }

            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task<IActionResult> RunTests(string language, string ProgramName)
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
    }
}