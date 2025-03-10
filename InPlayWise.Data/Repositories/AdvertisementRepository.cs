using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {

        private readonly AppDbContext _db;
        private readonly ILogger<AdvertisementRepository> _logger;
        public AdvertisementRepository(AppDbContext db, ILogger<AdvertisementRepository> logger) {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> AddAdvertisement(Advertisement ad)
        {
            try
            {
                await _db.Advertisements.AddAsync(ad);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteAdvertisement(Guid id)
        {
            try
            {
                Advertisement ad = await _db.Advertisements.FindAsync(id);
                _db.Remove(ad);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<Advertisement> GetAdvertisement(Guid id)
        {
            try
            {
                return await _db.Advertisements.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<Advertisement>> GetAllAdvertisements()
        {
            try
            {
                return await _db.Advertisements.OrderBy(ad => ad.StartTime).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<bool> UpdateAdvertisement(Advertisement ad)
        {
            try
            {
                Advertisement adv = await _db.Advertisements.FindAsync(ad.Id);
                _db.Entry(adv).CurrentValues.SetValues(ad);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
    }
}
