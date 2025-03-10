using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Route("[Controller]")]
    public class UpcomingMatchesController : ControllerBase
    {

        private readonly IUpcomingMatchesService _umService;
        public UpcomingMatchesController(IUpcomingMatchesService umService) {
            _umService = umService;
        }


        [HttpGet("GetAllMatches")]
        public async Task<IActionResult> GetAllUpcomingMatches()
        {
            return Ok(await _umService.GetAllUpcomingMatches());
        }



        [HttpGet("SearchUpcomingMatches")]
        public async Task<IActionResult> SearchUpcomingMatches(string query)
        {
            return Ok(await _umService.SearchUpcomingMatches(query));
        }



        [HttpGet("DeleteOldMatches")]
        public async Task<IActionResult> DeleteOldMatches()
        {
            return Ok(await _umService.DeleteOldMatches());
        }


    }
}
