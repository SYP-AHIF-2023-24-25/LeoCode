using System;
using System.Threading.Tasks;
using DotNetTestService.Core;
using DotNetTestService.Model;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTestService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CodeCheckController : ControllerBase
    {
        private readonly ICodeCheckService _service;

        public CodeCheckController(ICodeCheckService service)
        {
            this._service = service;
        }

        [HttpPost]
        [Route("submit")]
        public async Task<ActionResult<CheckReceivedResponse>> SubmitCodeForCheck([FromBody] EvaluationRequest request)
        {
            try
            {
                var checkId = await this._service.SubmitCodeForCheck(request.ProjectNo, request.CodeReplacements);
                return Ok(new CheckReceivedResponse
                {
                    ProjectNo = request.ProjectNo,
                    Id = checkId
                });
            }
            catch (UnknownProjectException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ReplacementCountNotMatchingException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:guid}/status")]
        public ActionResult<CheckStatusResponse> GetStatus([FromRoute] Guid id)
        {
            var runInfo = this._service.CheckRunStatus(id);
            if (runInfo == null)
            {
                return NotFound("unknown run id");
            }

            return Ok(new CheckStatusResponse
            {
                Status = runInfo.Status,
                FailReason = runInfo.FailReason,
                PassPercentage = runInfo.PassPercentage,
                AllPassed = runInfo.AllPassed ?? false,
                Duration = runInfo.Duration
            });
        }
    }
}