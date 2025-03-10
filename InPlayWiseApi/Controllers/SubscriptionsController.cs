using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace InPlayWiseApi.Controllers
{
    [Route("webhook")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class SubscriptionsController : Controller
    {
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "whsec_Np9YcqgBFaBFpLzOhOeJ6alJuejQ2uDj";

        /// <summary>
        /// Handles incoming Stripe webhook events.
        /// </summary>
        /// <returns>An IActionResult representing the result of webhook event handling.</returns>
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);


                switch (stripeEvent.Type)
                {
                    case Events.CustomerCreated:
                        // Handle CustomerCreated event
                        break;
                    case Events.CustomerDeleted:
                        // Handle CustomerDeleted event
                        break;
                    case Events.CustomerUpdated:
                        // Handle CustomerUpdated event
                        break;
                    case Events.SubscriptionScheduleCanceled:
                        // Handle SubscriptionScheduleCanceled event
                        break;
                    case Events.SubscriptionScheduleCompleted:
                        // Handle SubscriptionScheduleCompleted event
                        break;
                    case Events.SubscriptionScheduleCreated:
                        // Handle SubscriptionScheduleCreated event
                        break;
                    case Events.SubscriptionScheduleExpiring:
                        // Handle SubscriptionScheduleExpiring event
                        break;
                    case Events.SubscriptionScheduleUpdated:
                        // Handle SubscriptionScheduleUpdated event
                        break;
                    default:
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                        break;
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }
    }
}