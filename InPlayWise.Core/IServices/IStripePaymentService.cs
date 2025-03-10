using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IStripePaymentService
    {



        //Task<string> CreatePlan(string planName);
        //Task<string> CreatePrice(string productName, int priceInCents);
        //Task<string> GetPaymentLink(string priceId);

        /// <summary>
        /// Retrieves the count of users per subscription plan asynchronously.
        /// </summary>
        /// <returns>A dictionary containing the count of users per subscription plan.</returns>
        Task<Dictionary<String,int>> GetUsersPerPlanCountAsync();



    }
}
