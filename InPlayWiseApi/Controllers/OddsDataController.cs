using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
	//[Authorize]
	[Route("[Controller]")]
    [ApiController]
    public class OddsDataController : ControllerBase
    {

        private readonly IOddsDataService _oddsDataService;
        public OddsDataController(IOddsDataService oddsDataService)
        {
            _oddsDataService = oddsDataService;
        }

        [HttpGet("RealTimeOdds")]
        public async Task<IActionResult> RealTimeOdds()
        {
            return Ok(await _oddsDataService.GetRealTimeOdds());
        }

        [HttpGet("SingleMatchOdds")]
        public async Task<IActionResult> SingleMatchOdds(string matchId)
        {
            return Ok(await _oddsDataService.GetSingleMatchOdds(matchId));
        }

    }
}
