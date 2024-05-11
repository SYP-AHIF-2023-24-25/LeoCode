using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace csharp_runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOtherCSharpBackend", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            var app = builder.Build();

            app.UseCors("AllowOtherCSharpBackend");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapPost("/api/execute/{exerciseName}", RunTests)
                .WithName("RunTests");

            app.Run();
        }

        static async Task<IActionResult> RunTests(string exerciseName, [FromBody] JsonObject jsonContent)
        {
            try
            {
                Console.WriteLine("Unit Test für C# am ausführen");
                var currentDirectory = Directory.GetCurrentDirectory();
                Console.WriteLine("Current Directory: " + currentDirectory);
                Body body = JsonConvert.DeserializeObject<Body>(jsonContent.ToString());
                string templateFilePath = @$"{currentDirectory}/templates/{exerciseName}";
                Console.WriteLine($"Template File Path: {templateFilePath}");
                string filePathForRandomDirectory = @$"{currentDirectory}";
                string filePathForNuGetConfigFile = @$"{currentDirectory}/config";
                var result = await executeTests.runCSharp(exerciseName, templateFilePath, filePathForRandomDirectory, body.code, body.fileName);
                Console.WriteLine($"Result: {result}");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
    public class Body
    {
        public string code { get; set; }
        public string fileName { get; set; }
    }
}


