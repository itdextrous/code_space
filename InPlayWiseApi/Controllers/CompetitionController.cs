using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace InPlayWiseApi.Controllers
{
	[ApiController]
	[Route("[Controller]")]
	public class CompetitionController : ControllerBase
	{

		private readonly ICompetitionService _competitionService;
		public CompetitionController(ICompetitionService competitionService) {
			_competitionService = competitionService;
		}

		[HttpGet("GetFiftyCompetition")]
		public async Task<IActionResult> GetFiftyCompetition()
		{
			return Ok(await _competitionService.GetFiftyCompetiton());
		}

		[HttpGet("GetByName")]
		public async Task<IActionResult> GetCompetitionByName(string name)
		{
			return Ok(await _competitionService.GetCompetitionByName(name));
		}


		[HttpGet("GetByCountry")]
		public async Task<IActionResult> GetCompetitionByCountry(string countryName)
		{
			return Ok(await _competitionService.GetCompetitionByCountry(countryName));
		}

		[HttpPost("AddCategory")]
		public async Task<IActionResult > AddCategory()
		{
			return Ok(await _competitionService.AddCategory());
		}

		[HttpGet]
		public async Task<IActionResult> GetCompetition(string? id = null, bool league = false)
		{
			return Ok(await _competitionService.GetCompetition(id, league));
		}

        [HttpGet("TopLeagues")]
        public async Task<IActionResult> GetTopLeaguesAsync()
        {
            try
            {
                var result = await _competitionService.GetTopLeaguesAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
