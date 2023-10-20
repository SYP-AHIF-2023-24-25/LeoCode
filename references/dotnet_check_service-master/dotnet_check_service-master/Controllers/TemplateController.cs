using System.Threading.Tasks;
using DotNetTestService.Core;
using DotNetTestService.Model;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TemplateController : ControllerBase
    {
        private readonly ICodeCheckService _service;

        public TemplateController(ICodeCheckService service)
        {
            this._service = service;
        }

        [HttpGet]
        [Route("{projectNo:int}")]
        public async Task<ActionResult<ProjectTemplateResponse>> GetProjectTemplateFiles([FromRoute] int projectNo)
        {
            try
            {
                var files = await this._service.GetTemplateFiles(projectNo);
                var response = new ProjectTemplateResponse
                {
                    ProjectNo = projectNo,
                    Files = files
                };
                return Ok(response);
            }
            catch (UnknownProjectException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}