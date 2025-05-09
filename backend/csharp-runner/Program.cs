using Microsoft.AspNetCore.Mvc;
using System;
using System.IO; // Ergänzung
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Ergänzung
using Microsoft.Extensions.DependencyInjection; // Ergänzung
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text.Json.Nodes; // Ergänzung
using System.IO.Compression;
using System.Net.Http.Json;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

namespace csharp_runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseCors("AllowOrigin");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.MapPost("/api/execute/{exerciseName}", RunTests)
                    .WithName("RunTests");

            app.MapGet("/api/code", GetFirstCsFileContent)
                    .WithName("GetCode");

            app.MapGet("/api/test", () => "test");

            app.UseAuthorization();

            app.MapControllers();
            app.UsePathBase("/csharp-runner");
            app.Run();
        }

        public static async Task<IActionResult> GetFirstCsFileContent(string exerciseName)
        {
            try
            {
                string directoryPath = $"/app/templates/{exerciseName}/{exerciseName}/";

                if (!Directory.Exists(directoryPath))
                {
                    return new NotFoundObjectResult($"The directory {directoryPath} does not exist.");
                }

                string[] files = Directory.GetFiles(directoryPath);
                string[] csFiles = files.Where(file => file.EndsWith(".cs")).ToArray();

                if (csFiles.Length > 0)
                {
                    string csFilePath = csFiles[0];
                    string fileContent = await System.IO.File.ReadAllTextAsync(csFilePath);
                    //Console.WriteLine(fileContent);
                    return new OkObjectResult(fileContent);
                }
                else
                {
                    return new NotFoundObjectResult("No .cs file found in the specified directory");
                }
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        static async Task<IActionResult> RunTests(string exerciseName, [FromBody] JsonObject jsonContent)
        {
            Console.WriteLine("In Run Tests");
            Console.WriteLine(jsonContent);
            try
            {
                //Console.WriteLine("Unit Test für C# am ausführen");
                var currentDirectory = Directory.GetCurrentDirectory();
                //Console.WriteLine("Current Directory: " + currentDirectory);
                Body body = JsonConvert.DeserializeObject<Body>(jsonContent.ToString());
                string templateFilePath = @$"{currentDirectory}/templates/{exerciseName}";
                Console.WriteLine($"Template File Path: {templateFilePath}");
                string filePathForRandomDirectory = @$"{currentDirectory}";
                string filePathForNuGetConfigFile = @$"{currentDirectory}/config";
                Console.WriteLine($"File Path For Random Directory: {filePathForRandomDirectory}");
                Console.WriteLine($"File Path For NuGet Config File: {filePathForNuGetConfigFile}");
                var result = await executeTests.runCSharp(exerciseName, templateFilePath, filePathForRandomDirectory, body.code, body.fileName);
                //log succes
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                //log error
                //Console.WriteLine($"Error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }

    public class Body
    {
        public string code { get; set; }
        public string fileName { get; set; }
    }

    public class TemplateUploadModel
    {
        public string Content { get; set; }
        public IFormFile File { get; set; }
    }
}
