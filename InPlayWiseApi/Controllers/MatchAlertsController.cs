using InPlayWise.Common.DTO;
using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    public class MatchAlertsController : ControllerBase
    {

        private readonly IAlertsService _alertsService;

        public MatchAlertsController(IAlertsService alertsService) {
            _alertsService = alertsService;
        }

        [HttpPost("SetMatchAlertForUser")]
        public async Task<IActionResult> SetMatchAlertForUser([FromBody] MatchAlertRequestDto alert)
        {
            return Ok(await _alertsService.SetMatchAlert(alert));
        }



    }
}
