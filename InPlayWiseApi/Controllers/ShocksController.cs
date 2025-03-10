using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    public class ShocksController : ControllerBase
    {

        private readonly IShocksService _shockService;

        public ShocksController(IShocksService shockService) {
            _shockService = shockService;
        }


        [HttpGet("GetShockFactsOfMatch")]
        public async Task<IActionResult> GetShockFactsOfMatch(string matchId)
        {
            return Ok(await _shockService.GetShockFactsOfMatch(matchId));
        }



    }
}
