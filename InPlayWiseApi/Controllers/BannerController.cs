using InPlayWise.Common.DTO;
using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _topMsgService;
        public BannerController(IBannerService topMsgService)
        {
            _topMsgService = topMsgService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTopMessage(string? id = null)
        {
            var result = await _topMsgService.GetTopMessage(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddTopMessage(TopMessageDto dto)
        {
            var result = await _topMsgService.AddTopMessage(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditTopMessage(string id, [FromBody] TopMessageDto updatedMessage)
        {
            if (!Guid.TryParse(id, out var id2)) {
                return BadRequest("Unable to update message");
            }
            var result = await _topMsgService.EditTopMessage(updatedMessage, id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTopMessage(string id)
        {
            var result = await _topMsgService.DeleteTopMessage(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("FavouriteGames")]
        public async Task<IActionResult> GetFavouriteGames()
        {
            var result = await _topMsgService.GetFavouriteMatches();
            return StatusCode(result.StatusCode, result);
        }


    }
}
