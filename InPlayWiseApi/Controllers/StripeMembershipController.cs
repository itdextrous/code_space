using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{

    
    public class StripeMembershipController : ControllerBase
    {
        //private readonly IStripePaymentService _stripeService;
        //public StripeMembershipController(IStripePaymentService stripePaymentService)
        //{
        //    _stripeService = stripePaymentService;
        //}

        //[HttpPost("CreatePlan")]
        //public async Task<IActionResult> CreatePlan(string planName)
        //{
        //    return Ok(await _stripeService.CreatePlan(planName));
        //}

        //[HttpPost("CreatePrice")]
        //public async Task<IActionResult> CreatePrice(string productName, int priceInCents)
        //{
        //    return Ok(await _stripeService.CreatePrice(productName, priceInCents));
        //}

        //[HttpPost("GetPaymentLink")]
        //public async Task<IActionResult> GetPaymentLink(string priceId)
        //{
        //    return Ok(await _stripeService.GetPaymentLink(priceId));
        //}
    }
}
