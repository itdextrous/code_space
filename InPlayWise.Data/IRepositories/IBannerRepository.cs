using InPlayWise.Data.Entities;

namespace InPlayWise.Data.IRepositories
{
    public interface IBannerRepository
    {
        public Task<bool> AddTopMessage(TopMessageEntity msg);
        public Task<bool> UpdateTopMessage(TopMessageEntity msg);
        public Task<bool> DeleteTopMessage(Guid id);
        public Task<List<TopMessageEntity>> GetTopMessage(Guid id );

        Task<List<string>> GetFavouriteTeamsId(string userId);
    }
}
