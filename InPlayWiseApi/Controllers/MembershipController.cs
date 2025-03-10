using InPlayWise.Common.DTO;
using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService) {
            _membershipService = membershipService;
        }

        [HttpPost("CreateProduct")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto prod)
        {
            return Ok(await _membershipService.CreateProduct(prod));
        }

        //[Authorize(Roles = "admin")]
        [HttpPost("SetPriceOfProduct")]
        public async Task<IActionResult> SetPrice([FromBody] SetPriceDto prc)
        {
            return Ok(await _membershipService.SetPrice(prc));
        }

        [HttpGet("GetAllPlans")]
        public async Task<IActionResult> GetAllPlans()
        {
            return Ok(await _membershipService.GetAllPlans());
        }

        [HttpGet("GetPaymentLink")]
        public async Task<IActionResult> GetPaymentLink(string planId)
        {
            return Ok(await _membershipService.GetPaymentLink(planId));
        }

        [HttpPost("SetFeaturesForProduct")]
        public async Task<IActionResult> SetFeaturesForProduct(FeaturesDto featuresDto)
        {
            return Ok(await _membershipService.SetFeaturesForProduct(featuresDto));
        }

        [HttpGet("GetProductFeatures")]
        public async Task<IActionResult> GetFeaturesOfProduct(string productId)
        {
            return Ok(await _membershipService.GetProductFeatures(productId));
        }


        [HttpGet("GetAllStripePlans")]
        public async Task<IActionResult> GetAllStripePlans()
        {
            return Ok(_membershipService.GetAllApiPlans());
        }

        //[HttpPost("SyncPlansInDb")]
        //public async Task<IActionResult> SyncPansInDb()
        //{
        //    return Ok(await _membershipService.SyncPlansInDb());
        //}

        [HttpPost("SeedDefaultPlans")]
        public async Task<IActionResult> SyncDefaultPlans()
        {
            return Ok(await _membershipService.SyncDefaultPlans());
        }





        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(string productid)
        {
            return Ok(await _membershipService.DeleteProduct(productid));
        }

        [HttpDelete("DeletePrice")]
        public async Task<IActionResult> DeletePrice(string priceId)
        {
            return Ok(await _membershipService.DeletePrice(priceId));
        }

        



    }
}
