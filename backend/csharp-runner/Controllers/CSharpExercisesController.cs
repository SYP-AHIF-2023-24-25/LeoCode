using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Net.Http.Json;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace csharp_runner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CSharpExercisesController : ControllerBase
    {

        [HttpPost]
        [Route("UploadTemplate")]
        public async Task<IActionResult> UploadTemplate(IFormFile file, [FromForm] string content)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new BadRequestObjectResult("No file Uploaded");
                }
                if (string.IsNullOrEmpty(content))
                {
                    return new BadRequestObjectResult("No Description if Template is complete or empty");
                }
                var fileName = Path.GetFileName(file.FileName);
                var currentDirectory = Directory.GetCurrentDirectory();
                // Sicherstellen, dass der Ordner "templates" existiert
                string templatesDirectory = Path.Combine(currentDirectory, "templates");
                if (!Directory.Exists(templatesDirectory))
                {
                    Directory.CreateDirectory(templatesDirectory);
                }

                string templateFilePath = Path.Combine(templatesDirectory, fileName);
                string destinationFilePath = templatesDirectory;
                using (var fileStream = new FileStream(templateFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                ZipFile.ExtractToDirectory(templateFilePath, destinationFilePath);
                System.IO.File.Delete(templateFilePath);
                if (content == "full")
                {
                    try
                    {

                        // Use Path methods instead of string splitting for better reliability
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                        string directoryWithoutExtension = Path.GetFileNameWithoutExtension(templateFilePath);

                        Console.WriteLine(directoryWithoutExtension);
                        // Execute the tests and await the result
                        var resultTask = executeTests.testTemplate(directoryWithoutExtension, fileNameWithoutExtension);
                        var result = await resultTask;

                        // Serialize the result to JSON
                        var jsonResult = JsonConvert.SerializeObject(result);
                        Console.WriteLine(content);

                        Console.WriteLine("File uploaded10");

                        // Ensure the directory exists before attempting to delete it
                        if (Directory.Exists(directoryWithoutExtension))
                        {
                            Directory.Delete(directoryWithoutExtension, true);
                        }

                        // Log success and return the JSON result
                        return Ok(jsonResult);
                    }
                    catch (Exception ex)
                    {
                        // Log any errors that occur
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                        return StatusCode(StatusCodes.Status400BadRequest, "Something went wrong during the template processing.");
                    }
                }


                return Ok();
                


            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(StatusCodes.Status400BadRequest, "Something went wrong while Template upload");
            }
        }
    }
}
