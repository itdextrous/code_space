using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
namespace InPlayWiseApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class RecentMatchesController : ControllerBase
    {
        private readonly IRecentMatchService _recentMatchService;
        public RecentMatchesController(IRecentMatchService recentMatchService) {
            _recentMatchService = recentMatchService;
        }

        [HttpGet("GetMatchById")]
        public async Task<IActionResult> GetMatchById(string matchId)
        {
            return Ok(await _recentMatchService.MatchById(matchId));
        }

        [HttpGet("GetMatchesByTeamId")]
        public async Task<IActionResult> GetMatchesOfTeamById(string teamId)
        {
            return Ok(await _recentMatchService.MatchesOfTeamById(teamId));
        }

        [HttpGet("GetLastFiftyMatches")]
        public async Task<IActionResult> LastFiftyMatches()
        {
            return Ok(await _recentMatchService.Last50Matches());
        }

        [HttpGet("TeamMatchesByCompetitionId")]
        public async Task<IActionResult> GetMatchesByCompetition(string competitionId)
        {
            return Ok(await _recentMatchService.MatchesByCompetition(competitionId));
        }

        [HttpGet("LastThreeMatchesOfTeam")]
        public async Task<IActionResult> GetLastThreeMatchesOfTeam(string teamId)
        {
            return Ok(await _recentMatchService.GetLastThreeMatchesOfTeam(teamId));
        }

        [HttpGet("LastNMatchesOfTeam")]
        public async Task<IActionResult> GetLastNMatchesOfTeam(string teamId, int n)
        {
            return Ok(await _recentMatchService.GetLastNMatchesOfTeam(teamId, n));
        }

        //[HttpGet("SearchRecentMatch")]
        //public async Task<IActionResult> SearchRecentMatches(string query)
        //{
        //    return Ok(await _recentMatchService.SearchRecentMatches(query));
        //}

        [HttpGet("GetMatchesOfTeamByIdFromDb")]
        public async Task<IActionResult> GetMatchesOfTeamFromDb(string teamId)
        {
            return Ok(await _recentMatchService.GetMatchesOfTeamFromDbByTeamid(teamId));
        }

        [HttpGet("GetMatchFromDb")]
        public async Task<IActionResult> GetMatchFromDb(string matchid)
        {
            return Ok(await _recentMatchService.GetMatchFromDb(matchid));
        }

        [HttpPost("PostHistoricalMatchesOnLocal")]
        public async Task<IActionResult> PostHistoricalMatchesOnLocal()
        {
            return Ok(await _recentMatchService.GetHistoricalMatchesOnLocal());
        }


        [HttpGet("testpoint")]
        public async Task<IActionResult> TestPoint()
        {
            return Ok(await _recentMatchService.TestPoint());
        }

    }
}