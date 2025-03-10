using Google.Apis.Logging;
using InPlayWise.Common.Constants;
using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using System.Text.Json;
using File = System.IO.File;
using PriceModel = InPlayWise.Data.Entities.MembershipEntities.Price;
using ProductModel = InPlayWise.Data.Entities.MembershipEntities.Product;
using SubscriptionModel = InPlayWise.Data.Entities.MembershipEntities.Subscription;


namespace InPlayWise.Core.Services
{
	public class MembershipService : IMembershipService
	{
		private readonly IConfiguration _config;
		private readonly IMembershipRepository _membershipRepo;
		private readonly IHttpContextService _httpContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger<MembershipService> _logger;
		private static readonly List<string> subscriptionOrder = new List<string>
	   {
		   "free",
		   "Wise Edge",
		   "Wise Investor",
		   "Wise Platinum"
	   };

		public MembershipService(IMembershipRepository membershipRepository, IHttpContextService httpContext, UserManager<ApplicationUser> userManager, IConfiguration config, ILogger<MembershipService> logger)
		{
			_membershipRepo = membershipRepository;
			_httpContext = httpContext;
			_userManager = userManager;
			_config = config;
			_logger = logger;
		}
		public async Task<Result<bool>> CreateProduct(CreateProductDto prodDto)
		{
			try
			{
				if (string.IsNullOrEmpty(prodDto.ProductName)) return Result<bool>.BadRequest("Invalid plan name");

				ProductModel prodDb = await _membershipRepo.GetProductByName(prodDto.ProductName);
				if (prodDb is not null) { return Result<bool>.Conflict("Product with same name Exists"); };
				StripeList<Stripe.Product> stripeProducts = GetAllStripeProducts();

				//string prodName = StripeConstant.productPrefix + prodDto.ProductName.Trim();
				string prodName = prodDto.ProductName.Trim();


				//foreach (var stripeProduct in stripeProducts)
				//{
				//	if (stripeProduct.Name.Equals(prodName, StringComparison.OrdinalIgnoreCase))
				//	{
				//		return Result<bool>.Conflict("Product name already exists");
				//	}
				//	//if (stripeProduct.Name.StartsWith(StripeConstant.productPrefix + prodDto.ProductName))
				//	//	return Result<bool>.Conflict("Product name already exists");
				//}

				var productOptions = new ProductCreateOptions { Name = prodName };
				var productService = new ProductService();
				string productStripeId = productService.Create(productOptions).Id;
				ProductModel prod = new ProductModel
				{
					Id = new Guid(),
					Name = prodDto.ProductName,
					StripeId = productStripeId
				};
				await _membershipRepo.AddProduct(prod);

				return Result<bool>.Success("Plan created successfully");

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<bool>.InternalServerError();
			}
		}


		public async Task<string> GetFreePlanId()
		{
			try
			{
				return (await _membershipRepo.GetProductByName(StripeConstant.freePlanName)).Id.ToString();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<Result<bool>> SetPrice(SetPriceDto prc)
		{
			try
			{

				PriceModel pr = new PriceModel()
				{
					Id = Guid.NewGuid(),
					ProductId = Guid.Parse(prc.ProductId),
					IntervalInDays = prc.Recurring ? prc.DurationInDays : 3,
					PriceInCents = prc.AmountInCents
				};
				bool priceExists = await _membershipRepo.PriceExistsForProduct(pr);

				if ((prc.Recurring && prc.DurationInDays < 1) || string.IsNullOrEmpty(prc.ProductId) || prc.DurationInDays > 1000)
				{
					return priceExists ? Result<bool>.Conflict("There already exists a plan for same product and validity") : Result<bool>.BadRequest("Invalid parameters");
				}


				string productStripeId = (await _membershipRepo.GetProductById(pr.ProductId)).StripeId;
				var priceOptions = new PriceCreateOptions
				{
					Currency = "gbp",
					UnitAmount = pr.PriceInCents,
					Product = productStripeId
				};
				if (prc.Recurring)
				{
					priceOptions.Recurring = new PriceRecurringOptions { Interval = "day", IntervalCount = pr.IntervalInDays };
				}
				var priceService = new PriceService();
				var priceRes = priceService.Create(priceOptions);
				string stripePriceId = priceRes.Id;
				pr.StripeId = stripePriceId;
				await _membershipRepo.AddPrice(pr);
				return Result<bool>.Success("Price saved for plan");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<bool>.InternalServerError();
			}
		}
		public async Task<Result<List<ProductResponseDto>>> GetAllPlans()
		{
			try
			{

				List<ProductModel> products = await _membershipRepo.GetAllPrroducts();
				List<ProductResponseDto> result = new List<ProductResponseDto>();

				foreach (var product in products)
				{
					List<PriceModel> prices = await _membershipRepo.GetAllPriceOfProd(product.Id);
					List<PriceResponseDto> pricesDto = new List<PriceResponseDto>();
					foreach (var price in prices)
					{
						//if (price.PriceInCents == 100) continue;
						PriceResponseDto pr = new PriceResponseDto()
						{
							Id = price.Id,
							PriceInCents = price.PriceInCents,
							ValidityInDays = price.IntervalInDays
						};
						pricesDto.Add(pr);
					}

					ProductResponseDto prod = new ProductResponseDto()
					{
						Id = product.Id,
						Name = product.Name,
						Price = pricesDto
					};
					result.Add(prod);
				}

				return Result<List<ProductResponseDto>>.Success("", result);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Result<List<ProductResponseDto>>.InternalServerError();
			}
		}
		public Task<Result<string>> Subscribe(string priceId)
		{
			throw new NotImplementedException();
		}
		public async Task<Result<string>> GetPaymentLink(string planId)
		{
			try
			{
				Guid plan = Guid.Parse(planId);
				string stripePriceid = await _membershipRepo.GetStripePriceId(plan);
				string userEmail = (await GetUserEmail()).Items;
				var price = await _membershipRepo.GetPriceByStripeId(stripePriceid);

				if (stripePriceid is null || userEmail is null)
				{
					return new Result<string>
					{
						IsSuccess = false,
						Items = null,
						Message = "Invalid input",
						StatusCode = 400
					};
				}

				bool isTrial = price.PriceInCents == 100 || price.PriceInCents == StripeConstant.trialAmountInCents;

				var checkoutOptions = new Stripe.Checkout.SessionCreateOptions
				{
					SuccessUrl = _config.GetSection("StripeSettings:PaymentSuccessRedirectUrl").Value,
					CustomerEmail = userEmail,
					BillingAddressCollection = "auto",
					LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
					{
						new Stripe.Checkout.SessionLineItemOptions
						{
							Price = stripePriceid,
							Quantity = 1,
						},
					},
					Mode = isTrial ? "payment" : "subscription"
					
				};
				var checkoutService = new Stripe.Checkout.SessionService();
				var checkoutRes = checkoutService.Create(checkoutOptions);

				return Result<string>.Success("", checkoutRes.Url);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<string>.InternalServerError();
			}
		}
		private async Task<Result<string>> GetUserEmail()
		{
			try
			{
				string userId = _httpContext.GetUserId();
				if (userId is null) return Result<string>.BadRequest();

				var user = await _userManager.FindByIdAsync(userId);
				if (user is null) return Result<string>.NotFound("User not found");

				return Result<string>.Success("Successfully found email", user.Email);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<string>.InternalServerError();
			}
		}



		private async Task<Result<bool>> CreateSubscription(string userEmail, string priceId, string paymentIntentId)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(userEmail);
				if (user is null)
				{
					return Result<bool>.NotFound("User not found");
				}

				PriceModel price = await _membershipRepo.GetPriceByStripeId(priceId);
				string userId = await _userManager.GetUserIdAsync(user);
				var previousProduct = await _membershipRepo.GetProductByUserId(userId);
				string previousSubscription = previousProduct.Name;
				var currentProduct = await _membershipRepo.GetProductById(price.ProductId);
				string currentSubscription = (currentProduct.Name);
				string planStatus = CheckUpgradeOrDowngrade(currentSubscription, previousSubscription);
				bool upgrade = planStatus == "Upgrade";
				bool downgrade = planStatus == "Downgrade";

				if (price.PriceInCents == 0)
				{
					SubscriptionModel freeSubscription = new SubscriptionModel
					{
						Id = Guid.NewGuid(),
						ProductId = price.ProductId,
						UserId = userId,
						PreviousSubscription = previousSubscription,
						CurrentSubscription = (price.Product.Name),
						SubscriptionStart = DateTime.Now,
						SubscriptionEnd = DateTime.Now.AddDays(price.IntervalInDays),
						IsUpgrade = upgrade,
						IsDowngrade = downgrade
					};

					if (await _membershipRepo.SubscriptionExists(userId))
					{
						await _membershipRepo.UpdateSubscription(freeSubscription, user);
					}
					else
					{
						await _membershipRepo.AddSubscription(freeSubscription);
					}
					return Result<bool>.Success("Free Subscription Created");
				}

				var paymentIntentService = new PaymentIntentService();
				var paymentIntent = paymentIntentService.Get(paymentIntentId);

				var paymentMethodService = new PaymentMethodService();
				var paymentMethod = paymentMethodService.Get(paymentIntent.PaymentMethodId);

				string fingerPrint = paymentMethod.Card.Fingerprint;

				TrialCard card = new TrialCard()
				{
					Id = Guid.NewGuid(),
					CardFingerPrint = fingerPrint
				};

				if (price.PriceInCents == 100)
				{
					bool cardUsed = await _membershipRepo.CardIsUsed(card);
					if (cardUsed)
					{
						// Initiate refund
						bool refunded = await InitiateRefund(paymentIntentId);
						return Result<bool>.BadRequest("Same Card is already used for availing trial. Refund is initiated");
					}
				}

				SubscriptionModel subscription = new SubscriptionModel
				{
					Id = Guid.NewGuid(),
					ProductId = price.ProductId,
					UserId = userId,
					PreviousSubscription = previousSubscription,
					CurrentSubscription = (price.Product.Name),
					SubscriptionStart = DateTime.Now,
					SubscriptionEnd = DateTime.Now.AddDays(price.IntervalInDays),
					IsUpgrade = upgrade,
					IsDowngrade = downgrade
				};

				if (await _membershipRepo.SubscriptionExists(userId))
				{
					await _membershipRepo.UpdateSubscription(subscription, user);
				}
				else
				{
					await _membershipRepo.AddSubscription(subscription);
				}
				user.ProductId = subscription.ProductId.ToString();
				await _userManager.UpdateAsync(user);
				if (price.PriceInCents == 100)
				{
					user.IsTrialAvailed = true;
					user.TrialActive = true;
					bool cardAdded = await _membershipRepo.AddToSavedTrialCards(card);
					bool refunded = await InitiateRefund(paymentIntentId);
					await _userManager.UpdateAsync(user);
				}
				return new Result<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<bool>
				{
					IsSuccess = false,
					Items = false
				};
			}
		}


		private async Task<bool> InitiateRefund(string paymentIntentId)
		{
			try
			{
				var paymentIntentService = new PaymentIntentService();
				var paymentIntent = paymentIntentService.Get(paymentIntentId);

				// Check if the payment intent was successful
				if (paymentIntent.Status == "succeeded")
				{
					// Create a refund for the payment intent
					var refundOptions = new RefundCreateOptions
					{
						PaymentIntent = paymentIntentId,
					};
					var refundService = new RefundService();
					var refund = refundService.Create(refundOptions);

					if (refund.Status.Equals("succeeded"))
						return true;

					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}






		private async Task<bool> CreateFreeSubscriptionForUser(string userId)
		{
			try
			{
				SubscriptionModel subscription = new SubscriptionModel
				{
					Id = new Guid(),
					ProductId = (await _membershipRepo.GetProductByName("free")).Id,
					UserId = userId,
					SubscriptionStart = DateTime.Now,
					SubscriptionEnd = DateTime.Now.AddYears(100)
				};
				return await _membershipRepo.AddSubscription(subscription);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<Result<ProductModel>> GetUserSubscription()
		{
			try
			{

				string userId = _httpContext.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return new Result<ProductModel> { IsSuccess = false };
				}

				if (!(await _membershipRepo.SubscriptionExists(userId)))
				{
					await CreateFreeSubscriptionForUser(userId);
				}
				return new Result<ProductModel>
				{
					IsSuccess = true,
					Items = await _membershipRepo.GetProductByUserId(userId),
					StatusCode = 200
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<ProductModel>
				{
					IsSuccess = false,
					Message = "Internal server error",
					StatusCode = 500
				};
			}
		}
		public async Task<Result<bool>> SetFreeSubscription(string userId)
		{
			try
			{
				var freePlan = await _membershipRepo.GetProductByName("free");
				if (freePlan is null)
				{
                    ProductModel prod = new ProductModel
                    {
                        Id = new Guid(),
                        Name = "free",
                        StripeId = null
                    };
                    await _membershipRepo.AddProduct(prod);
     //               CreateProductDto fp = new CreateProductDto()
					//{
					//	ProductName = "free"
					//};
					//var created = await CreateProduct(fp);
				}

                freePlan = await _membershipRepo.GetProductByName("free");
                SubscriptionModel subscription = new SubscriptionModel()
				{
					Id = new Guid(),
					ProductId = freePlan.Id,
					UserId = userId,
					CurrentSubscription = (freePlan.Name),
					SubscriptionStart = DateTime.Now,
					SubscriptionEnd = DateTime.Now.AddYears(100)
				};

				await _membershipRepo.AddSubscription(subscription);
				return new Result<bool>
				{
					IsSuccess = true,
					Items = true,
					StatusCode = 200
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<bool>
				{
					IsSuccess = false,
					Message = "Internal server error",
					StatusCode = 500
				};
			}
		}
		public async Task<Result<bool>> SetFeaturesForProduct(FeaturesDto features)
		{
			try
			{
				bool featureExists = await _membershipRepo.FeatureExists(Guid.Parse(features.ProductId));
				PlanFeatures ft = new PlanFeatures()
				{
					Id = Guid.NewGuid(),
					ProductId = Guid.Parse(features.ProductId),
					LiveInsightPerGame = features.LiveInsightPerGame,
					LivePredictionPerGAme = features.LivePredictionPerGAme,
					MaxPredictions = features.MaxPredictions,
					AccumulatorGenerators = features.AccumulatorGenerators,
					ShockDetectors = features.ShockDetectors,
					CleverLabelling = features.CleverLabelling,
					HistoryOfAccumulators = features.HistoryOfAccumulators,
					WiseProHedge = features.WiseProHedge,
					LeagueStatistics = features.LeagueStatistics,
					WiseProIncluded = features.WiseProIncluded
				};
				if (!featureExists)
				{
					await _membershipRepo.AddFeature(ft);
				}
				else
				{
					await _membershipRepo.UpdateFeature(ft, Guid.Parse(features.ProductId));
				}

				return new Result<bool>
				{
					IsSuccess = true,
					StatusCode = 200,
					Message = featureExists ? "Successfully updated features" : "Successfully saved features"
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<bool>
				{
					IsSuccess = false,
					Message = "Internal server error",
					StatusCode = 500
				};
			}
		}

		public async Task<Result<PlanFeatures>> GetProductFeatures(string productId)
		{
			try
			{
				PlanFeatures ft = await _membershipRepo.GetProductFeatures(Guid.Parse(productId));
				return new Result<PlanFeatures>
				{
					IsSuccess = ft is not null,
					Items = ft is null ? new PlanFeatures() : ft,
					Message = ft is null ? "Invalid request" : "These are the features of this plan",
					StatusCode = ft is null ? 400 : 200
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<PlanFeatures>
				{
					IsSuccess = false,
					StatusCode = 500,
					Message = "Internal Server Error"
				};
			}
		}


		public async Task<bool> DailyRefreshMembershipStatus()
		{
			try
			{
				TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;
				bool condition = currentTime >= TimeSpan.Zero && currentTime < TimeSpan.FromMinutes(1);
				if (!condition)
				{
					return false;
				}

				_logger.LogInformation("resetting subscription for day - " + DateTime.UtcNow.ToShortDateString());

				List<SubscriptionModel> allSubscriptions = await _membershipRepo.GetAllSubscriptions();
				if (allSubscriptions is null)
				{
					_logger.LogError("Error in resetting subscriptions");
					return false;
				}

				List<SubscriptionModel> subscriptionsToReset = new List<SubscriptionModel>();
				var freePlanId = (await _membershipRepo.GetProductByName("free")).Id;
				foreach (SubscriptionModel sub in allSubscriptions)
				{
					SubscriptionModel updatedSub = new SubscriptionModel();
					if (sub.SubscriptionEnd > DateTime.UtcNow)
					{
						updatedSub.SubscriptionStart = DateTime.UtcNow;
						updatedSub.SubscriptionEnd = DateTime.UtcNow.AddYears(100);
						updatedSub.ProductId = freePlanId;
						subscriptionsToReset.Add(sub);
					}
					subscriptionsToReset.Add(updatedSub);
				}

				bool reset = await _membershipRepo.ResetSubscriptions(subscriptionsToReset);
				return reset;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in resetting subscriptions " + ex.ToString());
				return false;
			}
		}

		public object GetAllApiPlans()
		{
			try
			{
				var options = new ProductListOptions { Limit = 100 };
				var service = new ProductService();
				StripeList<Stripe.Product> products = service.List(options);
				return products;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}

		public async Task<bool> SyncPlansInDb()
		{
			try
			{

				// seeding products which are not present in db
				var productOptions = new ProductListOptions { Limit = 1000 };
				var productService = new ProductService();
				StripeList<Stripe.Product> products = productService.List(productOptions);

				foreach (var stripeProdcut in products)
				{
					string productName = stripeProdcut.Name;
					if (!productName.StartsWith(StripeConstant.productPrefix)) continue;
					ProductModel prodInDb = await _membershipRepo.GetProductByStripeId(stripeProdcut.Id);
					if (prodInDb is null)
					{
						ProductModel pm = new ProductModel()
						{
							Id = Guid.NewGuid(),
							Name = stripeProdcut.Name.Substring(StripeConstant.productPrefix.Length),
							StripeId = stripeProdcut.Id,
							SubscriptionId = new Guid()
						};
						bool uploadedProductToDb = await _membershipRepo.AddProduct(pm);
						if (!uploadedProductToDb)
							return false;
					}
				}

				// seeding prices which are not present in db
				var priceOptions = new PriceListOptions { Limit = 1000 };
				var priceService = new PriceService();
				StripeList<Stripe.Price> prices = priceService.List(priceOptions);

				foreach (var price in prices)
				{
					PriceModel priceInDb = await _membershipRepo.GetPriceByStripeId(price.Id);
					if (priceInDb is null)
					{
						ProductModel prod = await _membershipRepo.GetProductByStripeId(price.ProductId);
						if (prod is null) continue;
						int interval = price.Recurring is null ? 3 : int.TryParse(price.Recurring.Interval, out interval) ? interval : 77;
						PriceModel pm = new PriceModel()
						{
							Id = Guid.NewGuid(),
							StripeId = price.Id,
							ProductId = prod.Id,
							IntervalInDays = interval,
							PriceInCents = (int)price.UnitAmount
						};
						bool uploadedPriceToDb = await _membershipRepo.AddPrice(pm);
						if (!uploadedPriceToDb)
							return false;
					}
				}

				// Creating free product if doesn't exists
				ProductModel freeProd = await _membershipRepo.GetProductByName(StripeConstant.freePlanName);
				if(freeProd is null)
				{
                    var prodOptions = new ProductCreateOptions { Name = StripeConstant.productPrefix + StripeConstant.freePlanName };
                    var prodService = new ProductService();
                    string productStripeId = productService.Create(prodOptions).Id;
                    ProductModel prod = new ProductModel
                    {
                        Id = new Guid(),
                        Name = StripeConstant.freePlanName,
                        StripeId = productStripeId
                    };
                    await _membershipRepo.AddProduct(prod);

                    PriceModel pr = new PriceModel()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = prod.Id,
                        PriceInCents = 0
                    };
                    var prcOptions = new PriceCreateOptions
                    {
                        Currency = "gbp",
                        UnitAmount = pr.PriceInCents,
                        Product = productStripeId
                    };
                    var prcService = new PriceService();
                    var priceRes = priceService.Create(prcOptions);
                    string stripePriceId = priceRes.Id;
                    pr.StripeId = stripePriceId;
                    await _membershipRepo.AddPrice(pr);
                    return true;
                }

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}


		private StripeList<Stripe.Product> GetAllStripeProducts()
		{
			try
			{
				var productOptions = new ProductListOptions { Limit = 1000 };
				var productService = new ProductService();
				StripeList<Stripe.Product> products = productService.List(productOptions);
				return products;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}






		public async Task<bool> HandleWebhook(string json, Event stripeEvent)
		{
			try
			{
				if (stripeEvent.Type == Events.CheckoutSessionCompleted)
				{
                    var document = JsonDocument.Parse(json);
                    JsonElement root = document.RootElement;
                    JsonElement dataElement = root.GetProperty("data");
                    JsonElement dataObject = dataElement.GetProperty("object");

                    string sessionId = dataObject.GetProperty("id").ToString();
                    string email = dataObject.GetProperty("customer_email").ToString();
                    string expire = dataObject.GetProperty("expires_at").ToString();
                    string paymentIntentId = dataObject.GetProperty("payment_intent").ToString();
                    string subscriptionId = dataObject.TryGetProperty("subscription", out var subscriptionProperty)
                        ? subscriptionProperty.ToString()
                        : null;

                    string priceId;

                    if (!string.IsNullOrEmpty(subscriptionId))
                    {
                        // Handle recurring subscriptions
                        var subscriptionService = new SubscriptionService();
                        var subscription = subscriptionService.Get(subscriptionId);

                        // Get the Price ID from the subscription
                        priceId = subscription.Items.Data.First().Price.Id;

                        // Get the latest invoice's PaymentIntent ID
                        var invoiceService = new InvoiceService();
                        var latestInvoice = invoiceService.Get(subscription.LatestInvoiceId);
                        paymentIntentId = latestInvoice.PaymentIntentId;
                    }
                    else
                    {
                        // Handle one-time payments
                        var sessionService = new SessionService();
                        var lineItems = sessionService.ListLineItems(sessionId);

                        // Assuming there's only one item in the session
                        priceId = lineItems.Data.First().Price.Id;
                    }

                    await CreateSubscription(email, priceId, paymentIntentId);
				}
				else
				{
					Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
				}

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}
		

		public async Task<Result<bool>> DeleteProduct(string productId)
		{
			try
			{
				// Parse the productId to Guid
				Guid productGuid = Guid.Parse(productId);

				// Retrieve the product from the database
				ProductModel prodDb = await _membershipRepo.GetProductById(productGuid);
				if (prodDb == null)
				{
					return Result<bool>.NotFound("No product found");
				}

				// Initialize the Stripe product service
				var prodService = new ProductService();

				// Retrieve the Stripe product to ensure it exists
				Stripe.Product prod = prodService.Get(prodDb.StripeId);
				if (prod == null)
				{
					return Result<bool>.NotFound("No Stripe product found with the given ID");
				}

				// Delete the product from Stripe
				var deletedProd = prodService.Delete(prodDb.StripeId);


				// Delete the product from the database
				bool dbDeletionResult = await _membershipRepo.DeleteProduct(productGuid);
				if (!dbDeletionResult)
				{
					return Result<bool>.InternalServerError("Failed to delete the product from the database");
				}

				// Return a success result
				return Result<bool>.Success("");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while deleting the product");
				return Result<bool>.InternalServerError();
			}
		}


		public async Task<Result<bool>> DeletePrice(string priceId)
		{
			try
			{
				return Result<bool>.Success();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<bool>.InternalServerError();
			}
		}

		public static string CheckUpgradeOrDowngrade(string currentSubscription, string previousSubscription)
		{
			int currentIndex = subscriptionOrder.IndexOf(currentSubscription);
			int previousIndex = subscriptionOrder.IndexOf(previousSubscription);

			if (currentIndex == -1 || previousIndex == -1)
			{
				// Handle invalid subscription types
				return "Invalid subscription type";
			}

			if (currentIndex > previousIndex)
			{
				return "Upgrade";
			}
			else if (currentIndex < previousIndex)
			{
				return "Downgrade";
			}
			else
			{
				return "No change";
			}
		}

        public async Task<Result<bool>> SyncDefaultPlans()
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Static", "inplay_plans.json");
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File inplay_plans not found in Static folder.");
                }

				List<ProductModel> products = await _membershipRepo.GetAllPrroducts();
				if(products.Count() > 1 || products.Where(pr => pr.Name != "free").Count() > 0)
				{
					return Result<bool>.Conflict("More than one plans exists");
				}

                string json = await File.ReadAllTextAsync(filePath);
				PlansJsonDto plansJson = JsonSerializer.Deserialize<PlansJsonDto>(json);
				Console.WriteLine(plansJson);
				foreach(PlanJson plan in plansJson.Plans)
				{
					await CreateProduct(new CreateProductDto() { ProductName = plan.Name});
                    ProductModel prodDb = await _membershipRepo.GetProductByName(plan.Name);
					Guid prodId = prodDb.Id;
                    foreach (PriceJson price in plan.Prices)
					{
						await SetPrice(new SetPriceDto()
						{
							ProductId = prodDb.Id.ToString(),
							AmountInCents = price.AmountInCents,
							DurationInDays = price.DurationInDays,
							Recurring = price.Recurring
						});
					}
                    await SetFeaturesForProduct(new FeaturesDto()
                    {

                        ProductId = prodDb.Id.ToString(),
						LiveInsightPerGame = plan.Features.LiveInsightsPerGame,
						LivePredictionPerGAme = plan.Features.LivePredictionPerGame ,
						MaxPredictions = plan.Features.MaxPredictions ,
						AccumulatorGenerators = plan.Features.AccumulatorGenerator ,
						ShockDetectors = plan.Features.ShockDetectors ,
						CleverLabelling = plan.Features.CleverLabelling ,
						HistoryOfAccumulators = plan.Features.HistoryOfAccumulator ,
						WiseProHedge = plan.Features.WiseProHedge ,
						LeagueStatistics = plan.Features.LeagueStatistics ,
						WiseProIncluded = plan.Features.WiseProIncluded ,
					});
				}
				Console.WriteLine($"hello {plansJson.Plans.Count()}");
                return Result<bool>.Success("All Plans seeded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError(ex.ToString());
            }
        }
    }
}




