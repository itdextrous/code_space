using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    [EnableCors("AllowAllOrigins")]
    public class TeamsController : ControllerBase
    {

        private readonly ITeamsService _teamsService;
        public TeamsController(ITeamsService teamsService) {
            _teamsService = teamsService;
        }

        [HttpGet("GetFiftyTeams")]
        public async Task<IActionResult> GetFiftyTeams()
        {
            return Ok(await _teamsService.GetFiftyTeams());
        }

        [HttpGet("SearchTeamByName")]
        public async Task<IActionResult> SearchTeamByName(string name)
        {
            return Ok(await _teamsService.SearchTeamsByName(name));
        }


        [HttpGet("SearchTeamByCountry")]
        public async Task<IActionResult> SearchTeamByCountry(string countryName)
        {
            return Ok(await _teamsService.SearchTeamsByCountry(countryName));
        }

        [HttpGet("SearchTeamByCompetition")]
        public async Task<IActionResult> SearchTeamByCompetition(string competitionName)
        {
            return Ok(await _teamsService.SearchTeamsByCompetition(competitionName));
        }


        [HttpGet("GetTeamById")]
        public async Task<IActionResult> GetTeamById(string teamId)
        {
            return Ok(await _teamsService.GetTeamById(teamId));
        }


    }
}
