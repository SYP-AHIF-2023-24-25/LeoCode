using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SecondLeoCodeBackend 
{
    class Program
    {
        private static Process backendProcess;

        static void Main(string[] args)
        {
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

            app.MapPost("/start", StartDockerContainerWithEmptyTemplate)
                .WithName("Start")
                .WithOpenApi();

            app.Run();
        }

        static async void StartDockerContainerWithEmptyTemplate()
        {
            try
            {
                Console.WriteLine("Starting Docker Container...");

                var command = $"run -p 8090:80 zole";
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
                        Console.WriteLine("Docker Container started successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Error starting Docker Container. Exit code: {backendProcess.ExitCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}