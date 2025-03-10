using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{

    [Route("[Controller]")]
    [Authorize]
    [EnableCors("AllowAllOrigins")]
    public class CleverLabelsController : ControllerBase
    {

        private readonly ICleverLabelServices _cleverLabelServices;

        public CleverLabelsController(ICleverLabelServices cleverLabelServices)
        {
            _cleverLabelServices = cleverLabelServices;
        }

        [HttpGet("AllLabels")]
        public async Task<IActionResult> GetAllLabels(string teamId)
        {
            return Ok(await _cleverLabelServices.GetAllLabels(teamId));
        }



    }
}