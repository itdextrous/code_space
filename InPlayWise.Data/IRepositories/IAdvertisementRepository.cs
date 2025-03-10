using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;

namespace InPlayWise.Data.IRepositories
{
    public interface IAdvertisementRepository
    {
        Task<Advertisement> GetAdvertisement(Guid id);
        Task<List<Advertisement>> GetAllAdvertisements();
        Task<bool> AddAdvertisement(Advertisement ad);
        Task<bool> UpdateAdvertisement(Advertisement ad);
        Task<bool> DeleteAdvertisement(Guid id);

    }
}
