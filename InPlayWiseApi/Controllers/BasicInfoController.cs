using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InPlayWiseApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowAllOrigins")]
    public class BasicInfoController : Controller
    {

        private readonly IBasicInfoServices _basicInfoServices;

        public BasicInfoController(IBasicInfoServices basicInfoServices)
        {
            _basicInfoServices = basicInfoServices;
        }


        // GET: api/<BasicInfoController>
        [HttpGet("category")]
        public async Task<IActionResult> GetCategory()
        {
            return Ok(await _basicInfoServices.Category());
        }


        [HttpGet("country")]
        public async Task<IActionResult> GetCountry()
        {
            return Ok(await _basicInfoServices.Country());
        }

        [HttpGet("competition")]
        public async Task<IActionResult> competition(string competitionId = "")
        {
            return Ok(await _basicInfoServices.Competition(competitionId));
        }

        [HttpGet("team")]
        public async Task<IActionResult> Team(string uuid = "")
        {
            return Ok(await _basicInfoServices.Team(uuid));
        }

        //[HttpGet("player")]
        //public async Task<IActionResult> Player(string uuid = "")
        //{
        //    return Ok(await _basicInfoServices.Player(uuid));
        //}

        //[HttpGet("coach")]
        //public async Task<IActionResult> CoachList(string uuid = "")
        //{
        //    return Ok(await _basicInfoServices.Coach(uuid));
        //}

        //[HttpGet("referee")]
        //public async Task<IActionResult> Refereelist(string uuid = "")
        //{
        //    return Ok(await _basicInfoServices.Referee(uuid));
        //}

        //[HttpGet("venue")]
        //public async Task<IActionResult> Venuelist(string uuid = "")
        //{
        //    return Ok(await _basicInfoServices.Venue(uuid));
        //}

        [HttpGet("season")]
        public async Task<IActionResult> Season(string seasonId = "")
        {
            return Ok(await _basicInfoServices.Season(seasonId));
        }

        [HttpGet("stage")]
        public async Task<IActionResult> Stagelist(string uuid = "")
        {
            return Ok(await _basicInfoServices.Stage(uuid));
        }

        //[HttpGet("dataupdate")]
        //public async Task<IActionResult> DataUpdate()
        //{
        //    return Ok(await _basicInfoServices.DataUpdate());
        //}

    }
}
