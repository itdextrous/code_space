using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{

    [Route("[Controller]")]
    [Authorize]
    [EnableCors("AllowAllOrigins")]
    public class LiveMatchesController : ControllerBase
    {

        private readonly ILiveMatchService _liveMatchService;

        public LiveMatchesController(ILiveMatchService liveMatchService)
        {
            _liveMatchService = liveMatchService;
        }

        [HttpGet("InfoById")]
        public async Task<IActionResult> LiveMatchInfo(string matchId)
        {
            return Ok(await _liveMatchService.LiveMatchInfo(matchId));
        }

        [HttpGet("AllLiveMatches")]
        public IActionResult AllLiveMatches()
        {
            return Ok(_liveMatchService.GetAllLiveMatches());
        }

        [HttpGet("AllLiveMatchesBasicInfo")]
        public async Task<IActionResult> AllLiveMatchesBasicInfo()
        {
            return Ok(await _liveMatchService.AllLiveMatchesBasicInfo());
        }

        [HttpGet("LiveMatchBasicInfoById")]
        public async Task<IActionResult> LiveMatchBasicInfoById(string matchId)
        {
            return Ok(await _liveMatchService.GetLiveMatchBasicInfo(matchId));
        }

        [HttpGet("SearchMatches")]
        public async Task<IActionResult> SearchMathces(string query)
        {
            return Ok(await _liveMatchService.SearchMatches(query));
        }

        [HttpGet("LiveFilters")]
        public async Task<IActionResult> GetLiveMatchFilters()
        {
            return Ok(await _liveMatchService.AllLiveMatchesFilter());
        }

    }
}
