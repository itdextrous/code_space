using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlanFeatures = InPlayWise.Data.Entities.MembershipEntities.PlanFeatures;

namespace InPlayWise.Data.Repositories
{
    public class MembershipRepository : IMembershipRepository
	{
		private readonly AppDbContext _db;
		private readonly ILogger<MembershipRepository> _logger;
        private readonly UserManager<ApplicationUser> _userManager;


        public MembershipRepository(AppDbContext db, ILogger<MembershipRepository> logger, UserManager<ApplicationUser> userManager)
		{
			_db = db;
			_logger = logger;
			_userManager = userManager;

		}

		public async Task<bool> AddFeature(PlanFeatures features)
		{
			try
			{
				await _db.PlanFeatures.AddAsync(features);
				await _db.SaveChangesAsync();
				return true;
			}catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> AddPrice(Price price)
		{
			try
			{
				await _db.Prices.AddAsync(price);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> AddProduct(Product prod)
		{
			try
			{
				await _db.Products.AddAsync(prod);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> AddSubscription(Subscription subscription)
		{
			try
			{
				await _db.Subscriptions.AddAsync(subscription);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> FeatureExists(Guid featureId)
		{
			try
			{
				PlanFeatures ft = await _db.PlanFeatures.FindAsync(featureId);
				return ft is not null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<List<Price>> GetAllPriceOfProd(Guid productId)
		{
			try
			{
				List<Price> allPricesOfProd = await _db.Prices.Where(pr => pr.ProductId == productId).ToListAsync();
				return allPricesOfProd;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new List<Price>();
			}
		}

		public async Task<List<Product>> GetAllPrroducts()
		{
			try
			{
				return await _db.Products.Include(p => p.Price).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

        public async Task<Product> GetProductByStripeId(string stripeProductId)
        {
            try
            {
                return await _db.Products.Where(pr => pr.StripeId.Equals(stripeProductId)).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<Price> GetPriceByStripeId(string stripePriceid)
		{
			try
			{
				return await _db.Prices.Where(pr => pr.StripeId.Equals(stripePriceid)).SingleOrDefaultAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<Product> GetProductById(Guid productId)
		{
			try
			{
				return await _db.Products.Include(p => p.Price).FirstOrDefaultAsync(p => p.Id == productId);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<Product> GetProductByName(string productName)
		{
			try
			{
				return await _db.Products.FirstOrDefaultAsync(prod => prod.Name.Equals(productName));
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<Product> GetProductByUserId(string userId)
		{
			try
			{
				Subscription sub = await _db.Subscriptions.SingleOrDefaultAsync(sub => sub.UserId.Equals(userId));
				return await _db.Products.FindAsync(sub.ProductId);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<bool> SubscriptionExists(string userId)
		{
			try
			{
				Subscription sub = await _db.Subscriptions.SingleOrDefaultAsync(sub => sub.UserId.Equals(userId));
				return sub is not null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}



		public async Task<PlanFeatures> GetProductFeatures(Guid productId)
		{
			try
			{
				PlanFeatures ft = await _db.PlanFeatures.SingleOrDefaultAsync(ft => ft.ProductId == productId);
				return ft;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}


		public async Task<string> GetStripePriceId(Guid priceId)
		{
			try
			{
				var price = await _db.Prices.FindAsync(priceId) ;
				return price.StripeId;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<bool> PriceExistsForProduct(Price prc)
		{
			try
			{
				List<Price> prices = await _db.Prices.Where(pr => pr.ProductId.Equals(prc.ProductId) && 
				(pr.IntervalInDays.Equals(prc.IntervalInDays) || pr.PriceInCents.Equals(prc.PriceInCents))).ToListAsync();

				return prices.Count > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> UpdateFeature(PlanFeatures features, Guid productId)
		{
			try
			{
				PlanFeatures ft = await _db.PlanFeatures.FindAsync(productId);
				ft.LiveInsightPerGame = features.LiveInsightPerGame;
				ft.LivePredictionPerGAme = features.LivePredictionPerGAme;
				ft.AccumulatorGenerators = features.AccumulatorGenerators;
				ft.ShockDetectors = features.ShockDetectors;
				ft.CleverLabelling = features.CleverLabelling;
				ft.HistoryOfAccumulators = features.HistoryOfAccumulators;
				ft.WiseProHedge = features.WiseProHedge;
				ft.LeagueStatistics = features.LeagueStatistics;
				ft.WiseProIncluded = features.WiseProIncluded;
				await _db.SaveChangesAsync();
				return true;
			}catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}


		public async Task<bool> UpdateSubscription(Subscription subscription,ApplicationUser user)
		{
			try
			{
				Subscription sub = await _db.Subscriptions.SingleOrDefaultAsync(sub => sub.UserId.Equals(subscription.UserId));
				sub.ProductId = subscription.ProductId;
			    sub.CurrentSubscription= subscription.CurrentSubscription;
				sub.PreviousSubscription = subscription.PreviousSubscription;
		        sub.IsUpgrade = subscription.IsUpgrade;
				sub.IsDowngrade = subscription.IsDowngrade;
				sub.SubscriptionStart = subscription.SubscriptionStart;
				sub.SubscriptionEnd = subscription.SubscriptionEnd;
                user.ProductId = subscription.ProductId.ToString();
                await _userManager.UpdateAsync(user);
                await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

        public Task<List<Subscription>> GetAllSubscriptions()
        {
            try
            {
				return _db.Subscriptions.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<bool> ResetSubscriptions(List<Subscription> subscriptions)
        {
            try
            {

                List<Subscription> subscriptionsInDb = await _db.Subscriptions.ToListAsync();
                foreach (Subscription sub in subscriptions)
                {
                    foreach (var subDb in subscriptionsInDb)
                    {
                        if (sub.UserId.ToLower().Equals(subDb.UserId.ToLower()))
                        {
                            _db.Entry(subDb).CurrentValues.SetValues(sub);
                        }
                    }
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> AddToSavedTrialCards(TrialCard card)
        {
            try
            {
				await _db.TrialCards.AddAsync(card);
				await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> CardIsUsed(TrialCard card)
        {
			try
			{
				TrialCard cardInDb = await _db.TrialCards.SingleOrDefaultAsync(card => card.CardFingerPrint.Equals(card.CardFingerPrint));
				if (cardInDb is null)
					return false;
				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return true;
			}
        }

        public async Task<bool> DeleteProduct(Guid productGuid)
        {
            try
            {
                // Find the product by its Guid
                var product = await _db.Products.FindAsync(productGuid);

                if (product == null)
                {
                    // Product not found
                    return false;
                }

                // Remove the product from the context
                _db.Products.Remove(product);

                // Save the changes to the database
                await _db.SaveChangesAsync();

                // Return true to indicate successful deletion
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return false; // Return false to indicate failure
            }
        }

    }
}

