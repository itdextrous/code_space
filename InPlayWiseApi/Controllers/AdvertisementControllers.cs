using InPlayWise.Common.DTO;
using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
	[Route("[Controller]")]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _adService;

        public AdvertisementController(IAdvertisementService adService) {
            _adService = adService;
        }

        [Authorize(Roles ="user, admin")]
		[HttpGet("GetAdvertisement")]
        public async Task<IActionResult> GetAdvertisement(string? id = null)
        {
            var result = await _adService.GetAdvertisement(id);
            return StatusCode(result.StatusCode, result);
        }

		[Authorize(Roles = "admin")]
        [HttpPost("AddNewAdvertisement")]
        public async Task<IActionResult> AddNewAdvertisement(AdvertisementDto ad)
        {
            var result = await _adService.AddNewAdvertisement(ad);
            return StatusCode(result.StatusCode, result);
        }

		[Authorize(Roles = "admin")]
		[HttpPut("UpdateAdvertisement")]
        public async Task<IActionResult> UpdateAdvertisement(AdvertisementUpdateDto ad)
        {
            var result = await _adService.UpdateAdvertisement(ad);
            return StatusCode(result.StatusCode, result);
        }

		[Authorize(Roles = "admin")]
		[HttpDelete("DeleteAdvertisement")]
        public async Task<IActionResult> DeleteAdvertisement(string adId)
        {
            var result = await _adService.DeleteAdvertisement(adId);
            return StatusCode(result.StatusCode, result);
        }

		[Authorize(Roles = "admin")]
		[HttpGet("GetAllAdvertisements")]
        public async Task<IActionResult> GetAllAdvertisements()
        {
            var result = await _adService.GetAllAdvertisements();
            return StatusCode(result.StatusCode, result);
        }

    }
}
