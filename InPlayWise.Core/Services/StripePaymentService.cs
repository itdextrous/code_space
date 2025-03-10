using InPlayWise.Core.IServices;
using InPlayWiseCommon.Wrappers;
using Stripe;


namespace InPlayWise.Core.Services
{
    public class StripePaymentService : IStripePaymentService
    {

        //private readonly IStripeService _stripeService;
        //public StripePaymentService(IStripeService stripeService)
        //{
        //    _stripeService = stripeService;
        //}

        //public async Task<string> CreatePlan(string planName)
        //{
        //    return await _stripeService.CreatePlan(planName);
        //}

        //public async Task<string> CreatePrice(string productName, int priceInCents)
        //{
        //    return await _stripeService.CreatePrice(productName, priceInCents);
        //}

        //public async Task<string> GetPaymentLink(string priceId)
        //{
        //    return await _stripeService.GetPaymentLink(priceId);
        //}

        public async Task<Dictionary<string, int>> GetUsersPerPlanCountAsync()
        {
            // Dictionary to store the count of users per plan
            var usersPerPlan = new Dictionary<string, int>();

            try
            {
                // Initialize the Stripe subscription service
                var subscriptionService = new SubscriptionService();

                // Set options for listing subscriptions with a limit of 100 subscriptions per request
                var options = new SubscriptionListOptions { Limit = 100 };

                var subscriptions = await subscriptionService.ListAsync(options);

                foreach (var subscription in subscriptions)
                {
                    foreach (var item in subscription.Items.Data)
                    {
                        string planId = item.Plan.Id;
                        int numberOfUsers = (int)item.Quantity;

                        // Update the count of users per plan in the dictionary
                        usersPerPlan.TryGetValue(planId, out int currentCount);
                        usersPerPlan[planId] = currentCount + numberOfUsers;
                    }
                }
            }
            catch (StripeException ex)
            {
                // Log the StripeException or handle it appropriately
                throw new Exception("An error occurred while fetching user counts per plan.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle other exceptions
                throw new Exception("An error occurred while fetching user counts per plan.", ex);
            }
            return usersPerPlan;
        }


    }
}
