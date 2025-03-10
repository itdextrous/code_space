
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [EnableCors("AllowAllOrigins")]
    public class StripeController : Controller
    {
        //private readonly IStripeAppServices _stripeService;

        //public StripeController(IStripeAppServices stripeService)
        //{
        //    _stripeService = stripeService;
        //}

        /// <summary>
        /// Adds a new Stripe customer using the provided customer details.
        /// </summary>
        /// <param name="customer">The AddStripeCustomer object containing customer details.</param>
        /// <param name="ct">A cancellation token to cancel the operation if needed.</param>
        /// <returns>An ActionResult containing the newly created Stripe customer.</returns>
        //[HttpPost("customer/add")]
        //public async Task<ActionResult<StripeCustomer>> AddStripeCustomer(
        //    [FromBody] AddStripeCustomer customer,
        //    CancellationToken ct)
        //{
        //    // Call the StripeService to add a new Stripe customer
        //    StripeCustomer createdCustomer = await _stripeService.AddStripeCustomerAsync(
        //        customer,
        //        ct);
        //    var isCardValid = _stripeService.SaveCardDetail("xxxx", "123", "Testname");

        //    // Return a successful response with the created customer data
        //    return StatusCode(StatusCodes.Status200OK, createdCustomer);
        //}

        /// <summary>
        /// Adds a new Stripe payment using the provided payment details.
        /// </summary>
        /// <param name="payment">The AddStripePayment object containing payment details.</param>
        /// <param name="ct">A cancellation token to cancel the operation if needed.</param>
        /// <returns>An ActionResult containing the newly created Stripe payment.</returns>
        //[HttpPost("payment/add")]
        //public async Task<ActionResult<StripePayment>> AddStripePayment(
        //    [FromBody] AddStripePayment payment,
        //    CancellationToken ct)
        //{
        //    // Call the StripeService to add a new Stripe payment
        //    StripePayment createdPayment = await _stripeService.AddStripePaymentAsync(
        //        payment,
        //        ct);

        //    // Return a successful response with the created payment data
        //    return StatusCode(StatusCodes.Status200OK, createdPayment);
        //}





        //public async Task<IActionResult> GetPaymentLink(string planId)
        //{

        //}

    }
}
