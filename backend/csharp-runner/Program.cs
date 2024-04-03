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
                    builder.WithOrigins("http://localhost:7215")
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
                /*Console.WriteLine("drinen");
                Body body = JsonConvert.DeserializeObject<Body>(jsonContent.ToString());
                string templateFilePath = "./templates/" + exerciseName;
                var result = await executeTests.runCSharp(exerciseName, templateFilePath, body.code, body.fileName);*/
                return new OkObjectResult(null);
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


