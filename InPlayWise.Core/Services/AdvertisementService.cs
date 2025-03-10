using Azure.Storage.Blobs;
using InPlayWise.Common.DTO;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
	public class AdvertisementService : IAdvertisementService
    {

        private readonly IAdvertisementRepository _adRepo;
        private readonly ILogger<AdvertisementService> _logger;
		private readonly BlobServiceClient _blobServiceClient;
		private readonly BlobContainerClient _containerClient;
        private readonly MatchInMemoryService _inMemory;


		public AdvertisementService(IAdvertisementRepository repo, ILogger<AdvertisementService> logger, BlobServiceClient blobServiceClient, IConfiguration configuration, MatchInMemoryService inMemory)
        {
            _logger = logger;
            _adRepo = repo;
			_blobServiceClient = blobServiceClient;
			string containerName = configuration.GetValue<string>("BlobStorage:ContainerName");
			_containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			_containerClient.CreateIfNotExists();
            _inMemory = inMemory;
		}

        public async Task<Result<AdvertisementDto>> AddNewAdvertisement(AdvertisementDto newAd)
        {
            try
            {
                //            List<Advertisement> allAds = await _adRepo.GetAllAdvertisements();

                //foreach (Advertisement adv in allAds)
                //            {
                //                if(newAd.StartTime < adv.EndTime && newAd.EndTime > adv.StartTime){
                //                    return new Result<AdvertisementDto>(400, false, "The time slot is already taken", null);
                //                }
                //            }

                bool isImage = newAd.Img.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
                if (!isImage)
                    return Result<AdvertisementDto>.BadRequest("Inappropriate data type");

                string blobName = Guid.NewGuid() + newAd.Img.FileName;
                var blobClient = _containerClient.GetBlobClient(blobName);
                await blobClient.DeleteIfExistsAsync();
                await blobClient.UploadAsync(newAd.Img.OpenReadStream(), true);

                Advertisement addToSave = new Advertisement()
                {
                    Id = Guid.NewGuid(),
                    FirmLink = newAd.FirmLink,
                    ImgLink = blobName,
                    StartTime = newAd.StartTime,
                    EndTime = newAd.EndTime
                };

                bool saved = await _adRepo.AddAdvertisement(addToSave);
                return new Result<AdvertisementDto>(saved ? 200 : 500, saved, saved ? "Advertisement created successfully" : "Failed to save advertisement", saved ? newAd : null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<AdvertisementDto>(500, false, "Internal server error", null);
            }
        }




        public async Task<Result<bool>> DeleteAdvertisement(string adId)
        {
            try
            {
                bool deleted = await _adRepo.DeleteAdvertisement(Guid.Parse(adId));
                return new Result<bool>(deleted ? 200 : 500, deleted, deleted ? "Deleted successfully" : "Failed to delete", deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<bool>(500, false, "Internal server error", false);
            }
        }

        public async Task<Result<List<AdvertisementGetDto>>> GetAdvertisement(string? id = null)
        {
            try
            {
                if(id is not null)
                {
                    Advertisement adDb = await _adRepo.GetAdvertisement(Guid.Parse(id));
                    AdvertisementGetDto res = new AdvertisementGetDto()
                    {
                        StartTime = adDb.StartTime,
                        EndTime = adDb.EndTime,
                        FirmLink = adDb.FirmLink,
                    };
                    var blobClient = _containerClient.GetBlobClient(adDb.ImgLink);
                    var memoryStream = new MemoryStream();
                    await blobClient.DownloadToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var contentType = blobClient.GetProperties().Value.ContentType;
                    res.Img = memoryStream.ToArray();
                    return new Result<List<AdvertisementGetDto>>(200, true, "Current advertisement",
                        new List<AdvertisementGetDto> { res });
                }

                List<AdvertisementGetDto> ad = _inMemory.GetAdvertisement();
                if (ad is null)
                    return new Result<List<AdvertisementGetDto>>(404, false, "No advertisement", null);

                return new Result<List<AdvertisementGetDto>>(200, true, "Current advertisement", ad);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<List<AdvertisementGetDto>>(500, false, "Internal server error", null);
            }
        }

        public async Task<Result<List<Advertisement>>> GetAllAdvertisements()
        {
            try
            {
                var res = await _adRepo.GetAllAdvertisements();
                return new Result<List<Advertisement>>(res is not null ? 200 : 500, res is not null, res is not null ? "List of advertisements" : "Internal server error", res is not null ? res : new List<Advertisement>()); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<List<Advertisement>>(500, false, "Internal server error", new List<Advertisement>());
            }
        }

        public async Task<Result<bool>> UpdateAdvertisement(AdvertisementUpdateDto ad)
        {
            try
            {

                Advertisement existingAd = await _adRepo.GetAdvertisement(Guid.Parse(ad.Id));

                if (existingAd == null)
                {
                    return Result<bool>.NotFound("Advertisement not found.");
                }

                // 1. Conflict check (optional, uncomment if needed)
                // List<Advertisement> allAds = await _adRepo.GetAllAdvertisements();
                // foreach (Advertisement adv in allAds)
                // {
                //     if (ad.StartTime < adv.EndTime && ad.EndTime > adv.StartTime)
                //         return Result<bool>.Conflict("The time slot is already taken");
                // }

                bool updated = false;

                if (ad.Img is not null && ad.Img.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {

                    string blobName = Guid.NewGuid() + Path.GetExtension(ad.Img.FileName);
                    var blobClient = _containerClient.GetBlobClient(blobName);

                    await blobClient.DeleteIfExistsAsync();

                    await blobClient.UploadAsync(ad.Img.OpenReadStream(), overwrite: true);

                    existingAd.ImgLink = blobName;
                }

                existingAd.FirmLink = ad.FirmLink;
                existingAd.StartTime = ad.StartTime;
                existingAd.EndTime = ad.EndTime;

                updated = await _adRepo.UpdateAdvertisement(existingAd);

                return new Result<bool>(updated ? 200 : 500, updated, updated ? "Updated" : "Failed to update", updated);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError();
            }
        }


        public async Task<bool> UpdateInMemoryAds()
        {
            try
            {
                List<AdvertisementGetDto> ad = await FetchAdvertisement();
                _inMemory.SetAdvertisement(ad);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }


        private async Task<List<AdvertisementGetDto>> FetchAdvertisement()
        {
            try
            {
                DateTime currentTime = DateTime.UtcNow;
                List<Advertisement> allAds = await _adRepo.GetAllAdvertisements();
                List<AdvertisementGetDto> result = new List<AdvertisementGetDto>();
                if (allAds is null)
                    return null;

                foreach (Advertisement ad in allAds)
                {
                    if (ad.StartTime <= currentTime && ad.EndTime >= currentTime)
                    {
                        AdvertisementGetDto res = new AdvertisementGetDto()
                        {
                            StartTime = ad.StartTime,
                            EndTime = ad.EndTime,
                            FirmLink = ad.FirmLink,
                        };

                        var blobClient = _containerClient.GetBlobClient(ad.ImgLink);
                        var memoryStream = new MemoryStream();
                        await blobClient.DownloadToAsync(memoryStream);
                        memoryStream.Position = 0;
                        var contentType = blobClient.GetProperties().Value.ContentType;
                        res.Img = memoryStream.ToArray();
                        //return res;
                        result.Add(res);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }



    }
}
