using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    public class HistoricalStatsController : ControllerBase
    {

        private readonly IHistoricalStatsService _statsService;

        public HistoricalStatsController(IHistoricalStatsService serv) {
            _statsService = serv;
        }

        [HttpGet("GetStatsOfMatch")]
        public async Task<IActionResult> GetStatsOfMatch(string matchId)
        {
            return Ok(await _statsService.GetStatsOfMatch(matchId));
        }

		[HttpGet("GetStatsOfTeam")]
		public async Task<IActionResult> GetStatsOfTeam(string teamId)
		{
			return Ok(await _statsService.GetTeamStats(teamId));
		}

	}
}
