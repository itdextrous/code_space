using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Core.IServices;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IO;

namespace InPlayWiseApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class ProfileController : ControllerBase
    {

        private readonly IProfileService _profileService;
        private readonly IMembershipService _membershipService;

        public ProfileController(IProfileService profile, IMembershipService membershipService)
        {
            _profileService = profile;
            _membershipService = membershipService;
        }

        [HttpGet("GetUserTheme")]
        public async Task<IActionResult> ThemeDark()
        {
            return Ok(await _profileService.ThemeDark());
        }

        [HttpGet("SetDarkTheme")]
        public async Task<IActionResult> SetDarkTheme()
        {
            return Ok(await _profileService.SetDarkTheme());
        }

        [HttpGet("SetLightTheme")]
        public async Task<IActionResult> SetLightTheme()
        {
            return Ok(await _profileService.SetLightTheme());
        }

        [HttpGet("GetUserName")]
        public async Task<IActionResult> GetUserName()
        {
            return Ok(await _profileService.GetUserName());
        }

        [HttpGet("GetUserEmail")]
        public async Task<IActionResult> GetUserEmail()
        {
            return Ok(await _profileService.GetUserEmail());
        }

        [HttpPost("AddFavouriteTeam")]
        public async Task<IActionResult> AddFavouriteTeam(List<string> favouriteTeamsList)
        {
            return Ok(await _profileService.AddFavouriteTeams(favouriteTeamsList));
        }

        [HttpPost("AddFavouriteCompetition")]
        public async Task<IActionResult> AddFavouriteCompetition(List<string> competitionIdList)
        {
            return Ok(await _profileService.AddFavouriteCompetition(competitionIdList));
        }

        [HttpGet("GetFavouriteTeams")]
        public async Task<IActionResult> GetFavouriteTeams()
        {
            return Ok(await _profileService.GetFavouriteTeams());
        }

        [HttpGet("GetFavouriteCompetition")]
        public async Task<IActionResult> GetFavouriteCompetition()
        {
            return Ok(await _profileService.GetFavouriteCompetitions());
        }


        [HttpGet("RemoveFavouriteTeams")]
        public async Task<IActionResult> RemoveFavouriteTeams(List<string> teamIds)
        {
            return Ok(await _profileService.RemoveFavouriteTeams(teamIds));
        }
        [HttpGet("RemoveFavouriteCompetitions")]
        public async Task<IActionResult> RemoveFavouriteCompetitions(List<string> competitionIds)
        {
            return Ok(await _profileService.RemoveFavouriteCompetitions(competitionIds));
        }



        [HttpGet("GetUserSubscription")]
        public async Task<IActionResult> GetUserSubscription()
        {
            return Ok(await _membershipService.GetUserSubscription());
        }

        [HttpGet("GetUserQuota")]
        public async Task<IActionResult> GetUserQuota()
        {
            return Ok(await _profileService.GetUserQuota());
        }

        [HttpPost("SetAlerts")]
        public async Task<IActionResult> SetAlerts(SetAlertsDto dto)
        {
            return Ok(await _profileService.SetUserAlerts(dto));
        }

        [HttpGet("GetAlerts")]
        public async Task<IActionResult> GetAlerts()
        {
            return Ok(await _profileService.GetUserAlerts());
        }


        [HttpGet("IsTrialAvailed")]
        public async Task<IActionResult> IsTrialAvailed()
        {
            return Ok(await _profileService.IsTrialAvailed());
        }


        [HttpPost("SetCountry")]
        public async Task<IActionResult> SetCountry(CountryEnum country)
        {
            return Ok(await _profileService.SetCountry(country));
        }

        [HttpPost("GetCountry")]
        public async Task<Result<CountryEnum>> GetCountry()
        {
            return (await _profileService.GetCountry());
        }


        [HttpPost("SetProfilePic")]
        public async Task<IActionResult> SetProfilePic(IFormFile img)
        {
            return Ok(await _profileService.SetProfilePic(img));
        }

        [HttpGet("GetProfilePic")]
        public async Task<FileStreamResult> GetProfilePic()
        {
            return await _profileService.GetProfilePic();
        }

        [HttpPost("SetFullName")]
        public async Task<IActionResult> SetFullNameAsync(string firstName, string lastName)
        {
            return Ok(await _profileService.SetFullNameAsync(firstName, lastName));
        }

        [HttpGet("GetFullName")]
        public async Task<IActionResult> GetFullNameAsync()
        {
            return Ok(await _profileService.GetFullNameAsync());
        }

    }
}
