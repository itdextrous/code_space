using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    public class LeagueStatsController : ControllerBase
    {
        private readonly ILeagueStatsService _lsService;
        public LeagueStatsController(ILeagueStatsService lsService)
        {
            _lsService = lsService;
        }


        [HttpGet("GetTenLeaguesStats")]
        public async Task<IActionResult> GetTenLeagueStats()
        {
            return Ok(await _lsService.GetTenLeagueStats());
        }

        [HttpGet("GetLeagueStats")]
        public async Task<IActionResult> GetStatsOfLeage(string leagueId)
        {
            return Ok(await _lsService.GetStatsOfLeague(leagueId));
        }

        [HttpGet("GetLeagueStatsMultiple")]
        public async Task<IActionResult> GetStatsOfLeageMultiple(List<string> leagueIds)
        {
            return Ok(await _lsService.GetMultipleLeagueStats(leagueIds));
        }

        [HttpGet("GetLeagueStatsFromApi")]
        public async Task<IActionResult> GetLeagueStatsFromApi(string leagueId)
        {
            return Ok(await _lsService.GetStatsOfLeagueFromApi(leagueId));
        }

        [HttpGet("FavouriteLeagueStatsId")]
        public async Task<IActionResult> GetFavouriteLeagueStatsId()
        {
            var result = await _lsService.GetFavouriteLeagueStatIds();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("FavouriteLeagueStatsId")]
        public async Task<IActionResult> SetFavouriteLeagueStatsId(List<string> leagueIds)
        {
            var result = await _lsService.SetFavouriteLeagueStats(leagueIds);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("FavouriteLeagueStats")]
        public async Task<IActionResult> GetFavouriteLeagueStats()
        {
            var result = await _lsService.GetFavouriteLeagueStats();
            return StatusCode(result.StatusCode, result);
        }


    }
}
