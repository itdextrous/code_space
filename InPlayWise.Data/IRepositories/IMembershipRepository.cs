using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.MembershipEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IMembershipRepository
    {

        Task<List<Product>> GetAllPrroducts();
        Task<bool> AddProduct(Product prod);
        Task<bool> AddPrice(Price price);
        Task<Product> GetProductByName (string  productName);
        Task<Product> GetProductById(Guid productId);
        Task<Product> GetProductByUserId(string userId);
        Task<bool> PriceExistsForProduct(Price prc);
        Task<string> GetStripePriceId(Guid planId);
        Task<List<Price>> GetAllPriceOfProd(Guid productId);
        Task<Product> GetProductByStripeId(string stripeProductId);
        Task<Price> GetPriceByStripeId(string stripePriceId);
        Task<bool> AddSubscription(Subscription subscription);
        Task<bool> UpdateSubscription (Subscription subscription,ApplicationUser user);
        Task<bool> FeatureExists(Guid featureId);
        Task<bool> AddFeature(PlanFeatures features);
        Task<bool> UpdateFeature(PlanFeatures features, Guid productId);
        Task<PlanFeatures> GetProductFeatures(Guid productId);
        Task<bool> SubscriptionExists(string userId);

        Task<List<Subscription>> GetAllSubscriptions();
        Task<bool> ResetSubscriptions(List<Subscription> subscriptions);

        Task<bool> AddToSavedTrialCards(TrialCard card);
        Task<bool> CardIsUsed(TrialCard card);
       Task<bool> DeleteProduct(Guid productGuid);


    }
}

