using InPlayWise.Core.Annotations;
using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [EnableCors("AllowAllOrigins")]
    public class InsightsController : ControllerBase
    {
        private readonly IInsightService _insightService;

        public InsightsController(IInsightService insights) {
            _insightService = insights ;
        }

        [HttpGet("GetAllInsights")]
        public async Task<IActionResult> GetAllInsightsOfTeam(string teamId)
        {
            return Ok(await _insightService.GetAllInsightsOfTeam(teamId));
        }

	}
}
