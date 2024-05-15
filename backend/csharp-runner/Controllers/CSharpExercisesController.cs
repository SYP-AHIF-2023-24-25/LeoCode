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
                string templateFilePath = Path.Combine(currentDirectory, "templates", fileName);
                string destinationFilePath = Path.Combine(currentDirectory, "templates");
                using (var fileStream = new FileStream(templateFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                ZipFile.ExtractToDirectory(templateFilePath, destinationFilePath);

                System.IO.File.Delete(templateFilePath);
                if(content == "full")
                {
                    string[] parts = templateFilePath.Split(".");
                    string[] name = fileName.Split(".");
                    var resultTask = executeTests.testTemplate(parts[0], name[0]);
                    var result = await resultTask;
                    var jsonResult = JsonConvert.SerializeObject(result);
                    Console.WriteLine(content);
                    await resultTask;

                    Directory.Delete(parts[0], true);
                    //log succes
                    return Ok(jsonResult);
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
