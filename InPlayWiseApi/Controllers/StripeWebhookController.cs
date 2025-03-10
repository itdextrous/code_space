using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace InPlayWiseApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {

        private readonly IMembershipService _memberShipService;
        private readonly IConfiguration _config;

        public StripeWebhookController(IMembershipService memberShipService, IConfiguration config)
        {
            _memberShipService = memberShipService;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {           
            try
            {
                string endpointSecret = _config.GetSection("StripeSettings:WebhookEndpointSecret").Value;
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret, throwOnApiVersionMismatch:false);
                await _memberShipService.HandleWebhook(json, stripeEvent);
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }


    }
}
