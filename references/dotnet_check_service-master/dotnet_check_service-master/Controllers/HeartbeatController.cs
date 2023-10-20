using DotNetTestService.Core.Stats;
using DotNetTestService.Model;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class HeartbeatController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public HeartbeatController(IStatsService statsService)
        {
            this._statsService = statsService;
        }

        [HttpGet]
        public ActionResult<StatsResponse> GetStats()
        {
            return Ok(this._statsService.GetStats());
        }
    }
}