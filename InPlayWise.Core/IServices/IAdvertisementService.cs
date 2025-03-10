using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
	public interface IAdvertisementService
    {
        Task<Result<List<AdvertisementGetDto>>> GetAdvertisement(string id = null);
        Task<Result<AdvertisementDto>> AddNewAdvertisement(AdvertisementDto ad);
        Task<Result<bool>> UpdateAdvertisement(AdvertisementUpdateDto ad);
        Task<Result<bool>> DeleteAdvertisement(string adId);
        Task<Result<List<Advertisement>>> GetAllAdvertisements();
        Task<bool> UpdateInMemoryAds();


	}
}
