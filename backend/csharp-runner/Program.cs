using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace csharp_runner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/api/hello", () => "Hello World!");

            app.MapGet("/api/execute/:exerciseName", async (string exerciseName, [FromBody] JsonObject jsonBody) =>
            {
                Console.WriteLine("ininininini");
                Body body = JsonConvert.DeserializeObject<Body>(jsonBody.ToString());
                string templateFilePath = "./templates/" + exerciseName;
                var result = await executeTests.runCSharp(exerciseName, templateFilePath, body.code, body.fileName);
            });
            Console.WriteLine("ininininini");
            app.Run();
        }
    }

    public class Body
    {
        public string code { get; set; }
        public string fileName { get; set; }
    }
}
