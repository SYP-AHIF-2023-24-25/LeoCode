using Microsoft.AspNetCore.Mvc;
using System;
using System.IO; // Ergänzung
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Ergänzung
using Microsoft.Extensions.DependencyInjection; // Ergänzung
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text.Json.Nodes; // Ergänzung

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
                        builder.WithOrigins("http://localhost:4200") // Hier geben Sie die URL Ihrer Angular-Anwendung an
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            // Ergänzung: Add Controllers and NewtonsoftJson
            builder.Services.AddControllers();

            // Ergänzung: Add Antiforgery
            builder.Services.AddAntiforgery(options =>
            {
                // Konfigurationsoptionen hier einfügen, falls erforderlich
            });

            var app = builder.Build();

            app.UseCors("AllowOrigin");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Ergänzung: Use Routing
            app.UseRouting();

            // Ergänzung: Use Authentication and Authorization if needed
            // app.UseAuthentication();
            // app.UseAuthorization();

            // Ergänzung: Use Antiforgery
            app.UseAntiforgery();

            app.MapPost("/api/execute/{exerciseName}", RunTests)
                    .WithName("RunTests");
            app.MapPost("/api/uploadTemplate", UploadTemplate)
                .WithName("UploadTemplate");
            app.MapGet("/test", TEST)
                .WithName("TEST");

            app.Run();
        }
        static async Task<IActionResult> TEST()
        {
            return new OkObjectResult("AAAAAAAAAAAAAAAAAAAAAAAAA");
        }

        static async Task<IActionResult> UploadTemplate([FromForm] TemplateUploadModel model)
        {
            Console.WriteLine("AAAAAAAAAAAAA");
            Console.WriteLine("AAAAAAAAAAAAA");
            Console.WriteLine("AAAAAAAAAAAAA");
            Console.WriteLine(model.Content);
            Console.WriteLine(model.File.FileName);
            if (model == null || model.File == null || model.File.Length == 0)
            {
                // Handle invalid input (missing file or content)
                return new BadRequestObjectResult("Fail");
            }

            // Process the uploaded ZIP file (e.g., save it, extract its contents, etc.)
            // Access the file using 'model.File' and the content using 'model.Content'

            // Return an appropriate response (e.g., Ok, Created, etc.)
            return new OkObjectResult("Good");
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

    public class TemplateUploadModel
    {
        public string Content { get; set; }
        public IFormFile File { get; set; }
    }
}
