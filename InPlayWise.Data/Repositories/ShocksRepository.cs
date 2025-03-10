using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
	public class ShocksRepository : IShocksRepository
	{
		private readonly AppDbContext _db;
		private readonly ILogger<ShocksRepository> _logger;

		public ShocksRepository(AppDbContext dbContext, ILogger<ShocksRepository> logger)
		{
			_db = dbContext;
			_logger = logger;
		}

		public async Task<int> AddCounterIfDoesntExistsAndReturnHits(ShockCounter counter)
		{
			try
			{
				//  ShockCounter sc = await _db.ShockCounter.SingleOrDefaultAsync(co => co.UserId.Equals(counter.UserId) && co.Matchid.Equals(counter.Matchid));
				//            if (sc is not null)
				//            {
				//                sc.Count++;
				//                await _db.SaveChangesAsync();
				//                return sc.Count;
				//            }
				//            else
				//            {
				//	await _db.AddAsync(counter);
				//	await _db.SaveChangesAsync();
				//	return 1;
				//}
				return 1;

			}catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return -1; 
			}
		}

		public async Task<PlanFeatures> GetPlanFeatures(string userId)
		{
			try
			{
				Guid productId = (await _db.Subscriptions.SingleOrDefaultAsync(sub => sub.UserId.Equals(userId))).ProductId;
				PlanFeatures feature = await _db.PlanFeatures.SingleOrDefaultAsync(ft => ft.ProductId.Equals(productId));
				return feature;
			}catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return null;
			}
		}
	}
}
