using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{

	[Route("[Controller]")]
	public class SportsApiDataSeedController : ControllerBase
	{
		private readonly ISportsApiDataSeedService _dataSeed;
		public SportsApiDataSeedController(ISportsApiDataSeedService sportsApiDataSeedService) {
			_dataSeed = sportsApiDataSeedService;
		}

		[HttpPost("SeedCategories")]
		public async Task<IActionResult> SeedCategory()
		{
			return Ok(await _dataSeed.SeedCategory());
		}

		[HttpPost("SeedCountries")]
		public async Task<IActionResult> SeedCountry()
		{
			return Ok(await _dataSeed.SeedCountry());
		}

		[HttpPost("SeedCompetitions")]
		public async Task<IActionResult> SeedCompetitions()
		{
			return Ok(await _dataSeed.SeedCompetitions());
		}

		[HttpPost("SeedTeams")]
		public async Task<IActionResult> SeedTeams()
		{
			return Ok(await _dataSeed.SeedTeams());
		}

		//[HttpPost("SeedUpdatedTeams")]
		//public async Task<IActionResult> SeedUpdatedTeams()
		//{
		//	return Ok(await _dataSeed.SeedUpdatedTeams());
		//}

		//[HttpPost("SeedUpdatedTeamsAllBigData")]
		//public async Task<IActionResult> SeedAllTeams()
		//{
		//	return Ok(await _dataSeed.SeedAllTeamsUpdate());
		//}




		//[HttpPost("SeedUpdatedCompetitions")]
		//public async Task<IActionResult> SeedUpdatedCompetitions()
		//{
		//	return Ok(await _dataSeed.SeedUpdatedCompetitions());
		//}

		[HttpPost("SeedSeason")]
		public async Task<IActionResult> SeedSeason()
		{
			return Ok(await _dataSeed.SeedSeason());
		}

		[HttpPost("SeedUpcomingMatches")]
		public async Task<IActionResult> SeedUpcomingMatches()
		{
			return Ok(await _dataSeed.SeedUpcomingMatches());
		}


		[HttpPost("SeedTeamCount")]
		public async Task<IActionResult> UpdateTeamCount()
		{
			return Ok(await _dataSeed.SeedTeamCount());
		}

	}
}
