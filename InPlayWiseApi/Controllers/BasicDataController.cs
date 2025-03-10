using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [EnableCors("AllowAllOrigins")]
    public class BasicDataController : ControllerBase
    {
        private readonly IBasicDataServices _basicDataServices;
        public BasicDataController(IBasicDataServices basicDataServices)
        {
            _basicDataServices = basicDataServices;
        }

        [HttpGet("recentMatch")]
        public async Task<IActionResult> MatchRecent(string matchId = "", long timeStamp = 0)
        {
            return Ok(await _basicDataServices.MatchRecent(matchId, timeStamp));
        }


        [HttpGet("scheduleResultsSeason")]
        public async Task<IActionResult> ScheduleandResultsSeasonQuerry(string seasonId = "")
        {
            return Ok(await _basicDataServices.ScheduleAndResultsSeasonQuery(seasonId));
        }

        [HttpGet("RealTimeData")]
        public async Task<IActionResult> RealTimeData()
        {
            return Ok(await _basicDataServices.RealTimeData());
        }

        [HttpGet("RealTimeDataTest")]
        public async Task<IActionResult> RealTimeDataTest()
        {
            return Ok(await _basicDataServices.RealTimeDataTest());
        }

        [HttpGet("football/match/live/history")]
        public async Task<IActionResult> StatisticalDataHistorical(string matchId = "")
        {
            return Ok(await _basicDataServices.StatisticalData(matchId));
        }

        [HttpGet("GetIPAddress")]
        public async Task<IActionResult> GetIP()
        {
            string url = "https://api.thesports.com/v1/ip/demo";
            

            
             return (Ok(await _basicDataServices.GetIpAddress(url)));
            // Read the response content as a string
            
        }


    }
}
