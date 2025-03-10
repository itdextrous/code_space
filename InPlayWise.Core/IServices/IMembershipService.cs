using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWiseCommon.Wrappers;
using Stripe;
using ProductModel = InPlayWise.Data.Entities.MembershipEntities.Product;

namespace InPlayWise.Core.IServices
{
	public interface IMembershipService
	{
		Task<Result<bool>> CreateProduct(CreateProductDto product);
		Task<Result<bool>> SetPrice(SetPriceDto prc);
		Task<Result<List<ProductResponseDto>>> GetAllPlans();
		Task<Result<string>> GetPaymentLink(string planId);
		Task<Result<string>> Subscribe(string priceId);
		Task<Result<bool>> SetFreeSubscription(string userId);
		Task<Result<ProductModel>> GetUserSubscription();
		Task<Result<bool>> SetFeaturesForProduct(FeaturesDto features);
		Task<Result<PlanFeatures>> GetProductFeatures(string productId);
		Task<bool> HandleWebhook(string json, Event stripeEvent);
		Task<bool> DailyRefreshMembershipStatus();
		object GetAllApiPlans();

		Task<Result<bool>> SyncDefaultPlans();

		Task<bool> SyncPlansInDb();
		Task<Result<bool>> DeleteProduct(string productId);
		Task<Result<bool>> DeletePrice(string priceId);

		Task<string> GetFreePlanId();


    }
}
