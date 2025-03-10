using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;

namespace InPlayWise.Data.Repositories
{
    public class BannerRepository : IBannerRepository
    {

        private readonly AppDbContext _db;

        public BannerRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<bool> AddTopMessage(TopMessageEntity msg)
        {
            try
            {
                await _db.TopMessages.AddAsync(msg);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteTopMessage(Guid id)
        {
            try
            {
                TopMessageEntity msg = await _db.TopMessages.FindAsync(id);
                if (msg == null) return true;
                _db.TopMessages.Remove(msg);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<string>> GetFavouriteTeamsId(string userId)
        {
            try
            {
                return await _db.FavouriteTeams.Where(ft => ft.UserId.Equals(userId)).Select(us => us.TeamId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<string>();
            }
        }

        public async Task<List<TopMessageEntity>> GetTopMessage(Guid id = new Guid())
        {
            try
            {
                if(id == new Guid())
                {
                    return await _db.TopMessages.ToListAsync();
                }
                TopMessageEntity msg =  await _db.TopMessages.FindAsync(id);
                if (msg == null) { return new List<TopMessageEntity>(); }
                return new List<TopMessageEntity>() { msg };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<TopMessageEntity>();
            }
        }

        public async Task<bool> UpdateTopMessage(TopMessageEntity msg)
        {
            try
            {
                TopMessageEntity msgDb = await _db.TopMessages.FindAsync(msg.Id);
                if (msg == null) return true;
                _db.Entry(msgDb).CurrentValues.SetValues(msg);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
