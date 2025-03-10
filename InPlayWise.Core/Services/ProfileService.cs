using Azure.Storage.Blobs;
using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Diagnostics.Metrics;
using System.Security.Claims;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace InPlayWise.Core.Services
{
	public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager ;
        private readonly IHttpContextService _httpContext;
        private readonly IProfileRepository _profileRepo;
        private readonly ILogger<ProfileService> _logger;
		private readonly BlobServiceClient _blobServiceClient;
		private readonly BlobContainerClient _containerClient;

		public ProfileService( UserManager<ApplicationUser> userManager, IHttpContextService httpContext, IProfileRepository profileRepo, ILogger<ProfileService> logger, BlobServiceClient blobServiceClient, IConfiguration configuration) {
            _userManager = userManager;
            _httpContext = httpContext;
            _profileRepo = profileRepo;
            _logger = logger;
			_blobServiceClient = blobServiceClient;
            string containerName = configuration.GetValue<string>("BlobStorage:ContainerName");
			_containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			_containerClient.CreateIfNotExists();
		}

        public async Task<Result<bool>> AddFavouriteCompetition(List<string> competitionIdList)
        {
            try
            {
                string userId = _httpContext.GetUserId();
                await _profileRepo.AddFavouriteCompetition(userId, competitionIdList);
                return new Result<bool>
                {
                    IsSuccess = true,
                    Items = true,
                    StatusCode = 200,
                    Message = "Successfully added the competition to favourite"
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

        public async Task<Result<bool>> AddFavouriteTeams(List<string> teamIdList)
        {
            try
            {
                if (teamIdList.Count == 0)
                {
                    return Result<bool>.BadRequest("teamIdsList Can't be empty");
                }
                string userId = _httpContext.GetUserId();
                var existingFavouriteTeamIds = await _profileRepo.GetFavouriteTeamIds(userId);
                if (existingFavouriteTeamIds != null)
                {
                    teamIdList.RemoveAll(teamId => existingFavouriteTeamIds.Contains(teamId));
                }
                
                await _profileRepo.AddFavouriteTeam(userId, teamIdList);
                return Result<bool>.Success("FavouriteTeam is Added Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<bool>.InternalServerError("Error occured in Saving Favourite Team");
            }
        }

        public async Task<Result<List<Competition>>> GetFavouriteCompetitions()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                List<Competition> favComps = await _profileRepo.GetFavouriteCompetitions(userId);
                return new Result<List<Competition>>
                {
                    IsSuccess = true,
                    Items = favComps,
                    StatusCode = 200,
                    Message = "Successfully added team"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<List<Competition>>
                {
                    IsSuccess = false,
                    Message = "Internal server error",
                    StatusCode = 500
                };
            }
        }

        public async Task<Result<List<Team>>> GetFavouriteTeams()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                List<Team> favTeams = await _profileRepo.GetFavouriteTeams(userId);
                return new Result<List<Team>>
                {
                    IsSuccess = true,
                    Items = favTeams,
                    StatusCode = 200,
                    Message = "Successfully added team"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<List<Team>>
                {
                    IsSuccess = false,
                    Message = "Internal server error",
                    StatusCode = 500
                };
            }
        }

        public async Task<Result<SetAlertsDto>> GetUserAlerts()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                SetAlertsDto res = new SetAlertsDto()
                {
                    Email = user.EmailAlerts,
                    Desktop = user.DesktopAlerts
                };
                return new Result<SetAlertsDto>(200, true, "User alert choices", res);

            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<SetAlertsDto>(500, false, "Internal Server Error", null );
            }
        }

        public Task<Result<Common.Enums.CountryEnum>> GetUserCountry()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> GetUserEmail()
        {
            try
            {
                //if (_httpContext is null)
                //{
                //    return new Result<string>
                //    {
                //        IsSuccess = false,
                //        StatusCode = 400,
                //        Message = "Bad Request"
                //    };
                //}
                //var userIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = _httpContext.GetUserId();
                if (userId is null)
                {
                    return new Result<string>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Bad Request"
                    };
                }
    
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return new Result<string>
                    {
                        IsSuccess = false,
                        Message = "User not found",
                        StatusCode = 404
                    };
                }
                return new Result<string>
                {
                    IsSuccess = true,
                    Items = user.Email,
                    StatusCode = 200,
                    Message = "Successfully found email"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<string>
                {
                    IsSuccess = false,
                    Message = "Internal server error",
                    StatusCode = 500
                };
            }
        }

        public Task<Result<Language>> GetUserLanguage()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> GetUserName()
        {
            try
            {
                //var userIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = _httpContext.GetUserId();
                if (userId is null)
                {
                    return new Result<string>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Bad Request"
                    };
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return new Result<string>
                    {
                        IsSuccess = false,
                        Message = "User not found",
                        StatusCode = 404
                    };
                }
                return new Result<string>
                {
                    IsSuccess = true,
                    Items = user.UserName,
                    StatusCode = 200,
                    Message = "Successfully found UserName"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<string>
                {
                    IsSuccess = false,
                    Message = "Internal server error",
                    StatusCode = 500
                };
            }
        }

        public async Task<Result<UserQuota>> GetUserQuota()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                int predQuota = 0, shockDetectorsQuota = 0, leagueStatsQuota = 0, cleverLabelQuota = 0;
                int predCount = 0, shockCount = 0;
                PlanFeatures feat = await _profileRepo.GetPlanFeatures(userId);
                List<PredictionCounter> predCountList = await _profileRepo.GetPredictionRequestsOfUser(userId);
                if (predCountList is null) predQuota = 0;
                else {
                    foreach (PredictionCounter pcl in predCountList) {
                        predCount += pcl.Hits;
                    }
                    predQuota = feat.MaxPredictions - predCount;
                }

                List<ShockCounter> shockCountList = await _profileRepo.GetShockCounterRequestOfUser(userId);
                if (shockCountList is null)
                    shockDetectorsQuota = 0;
                else
                {
                    foreach (var sc in shockCountList) shockCount += sc.Count;
                    shockDetectorsQuota = feat.ShockDetectors - shockCount;
                }
                    
                List<CleverLabelsCounter> cleverLabelCountList = await _profileRepo.GetCleverLabelsRequestOfUser(userId);
                if (cleverLabelCountList is null) cleverLabelQuota = 0;
                else
                    cleverLabelQuota = feat.CleverLabelling - cleverLabelCountList.Count;
                
                List<LeagueStatsCount> leagueStatsCountList = await _profileRepo.GetLeagueStatsRequestOfUser(userId);
                if (leagueStatsCountList is null) leagueStatsQuota = 0;
                else
                    leagueStatsQuota = feat.LeagueStatistics - leagueStatsCountList.Count;
                UserQuota quota = new UserQuota()
                {
                    TotalPrediction = feat.MaxPredictions - predCount,
                    AccumulatorGenerators = 0,
                    ShockDetectors = 0,
                    CleverLabels = cleverLabelQuota,
                    HistoryOfAccumulators = 0,
                    WiseProHedge = 0,
                    LeagueStatistics = leagueStatsQuota,
                    WiseProIncluded = feat.WiseProIncluded
                };
                return new Result<UserQuota>(200, true, "UserQuotaFound", quota);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<UserQuota>(500, false, "Internal Server Error", null);
            }
        }

        public async Task<Result<bool>> RemoveFavouriteCompetitions(List<string> competitionIds)
        {
            try
            {
                bool removed = await _profileRepo.DeleteFavouriteCompetitions(_httpContext.GetUserId(), competitionIds);
                return new Result<bool>(removed ? 200 : 500, removed, removed ? "Removed successfully" : "Failed to remove", removed);
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<bool>(500, false, "Internal Server Error", false); 
            }
        }

        public async Task<Result<bool>> RemoveFavouriteTeams(List<string> teamIds)
        {
            try
            {
                bool removed = await _profileRepo.DeleteFavouriteTeams(_httpContext.GetUserId(), teamIds);
                return new Result<bool>(removed ? 200 : 500, removed, removed ? "Removed successfully" : "Failed to remove", removed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<bool>(500, false, "Internal Server Error", false);
            }
        }

        public Task<Result<bool>> SetCountry()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> SetDarkTheme()
        {
            try
            {
                if (_httpContext is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = false,
                        StatusCode = 400,
                        Message = "Bad Request"
                    };
                }
                //var userIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = _httpContext.GetUserId();
                if (userId is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = false,
                        StatusCode = 400,
                        Message = "Bad Request"
                    };
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = true,
                        Message = "User not found" ,
                        StatusCode = 404
                    };
                }
                user.ThemeIsDark = true;
                await _userManager.UpdateAsync(user);
                return new Result<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Successfully set the dark theme"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<bool>
                {
                    IsSuccess = false,
                    Items = true,
                    Message = "Internal server error",
                    StatusCode = 500
                };
            }
        }

        public Task<Result<bool>> SetLanguage()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> SetLightTheme()
        {
            try
            {
                if (_httpContext is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = false,
                        StatusCode = 400,
                        Message = "Bad Request"
                    };
                }
                //var userIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = _httpContext.GetUserId();
                if (userId is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = false,
                        StatusCode = 400,
                        Message = "Bad Request"
                    };
                }
                
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = true,
                        Message = "User not found",
                        StatusCode = 404
                    };
                }
                user.ThemeIsDark = false ;
                await _userManager.UpdateAsync(user);
                return new Result<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Set light theme successfully"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<bool>
                {
                    IsSuccess = false,
                    Items = true,
                    Message = "Internal server error",
                    StatusCode = 500
                };
            }
        }

        public async Task<Result<bool>> SetUserAlerts(SetAlertsDto dto)
        {
            try
            {
                string userId = _httpContext.GetUserId();
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                user.EmailAlerts = dto.Email;
                user.DesktopAlerts = dto.Desktop;
                await _userManager.UpdateAsync(user);
                return new Result<bool>(200, true, "Successfully updated", true);

            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<bool>(500, false, "Internal Server Error", false);
            }
        }

        public async Task<Result<bool>> ThemeDark()
        {
            try
            {
                if (_httpContext is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = false,
                        StatusCode = 400,
                        Message = "Bad Request"
                    };
                }
                //var userIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = _httpContext.GetUserId();
                if (userId is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = false,
                        StatusCode = 400,
                        Message = "Bad Request"
                    };
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Items = true,
                        Message = "User not found",
                        StatusCode = 404
                    };
                }
                bool isDark = user.ThemeIsDark;
                return new Result<bool>
                {
                    IsSuccess = true,
                    Items = isDark,
                    StatusCode = 200,
                    Message = isDark ? "Dark theme" : "Light Theme"
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<bool>
                {
                    IsSuccess = false,
                    Items = true,
                    Message = "Unable to fetch the theme hence default used",
                    StatusCode = 500
                };
            }
        }

        public async Task<Result<bool>> IsTrialAvailed()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                if (userId == null)
                {
                    // Unable to get the user ID
                    return Result<bool>.BadRequest("user id was invalid");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    // User not found
                    return Result<bool>.BadRequest("User was not found");
                }

                // Return the value of IsTrialAvailed
                return Result<bool>.Success("",user.IsTrialAvailed);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                return Result<bool>.InternalServerError("");
            }
        }

        public async Task<Result<bool>> SetCountry(CountryEnum country)
        {
            //int n = 10;
            //if (string.IsNullOrEmpty(countryName))
            //{
            //    return Result<bool>.BadRequest("Country name cannot be empty");
            //}
            try
            {
                string userId = _httpContext.GetUserId();
                var user = await _userManager.FindByIdAsync(userId);
                user.Country = country;
                await _userManager.UpdateAsync(user);

                return Result<bool>.Success("Country set successfuly");
                //// Convert country name to enum value
                //if (Enum.TryParse(typeof(CountryEnum), countryName, out object result))
                //{
                //    //Countries country = (Countries)result;
                //    //Countries[] values = (Countries[])Enum.GetValues(typeof(Countries));
                //    //int index = Array.IndexOf(values, country);

                //    //await _profileRepo.SetCountry(index);
                //    CountryEnum enumVal = (CountryEnum)result;


                //    return Result<bool>.Success();
                //}
                //else
                //{
                //    // Handle invalid country name
                //    return null;
                //}


            }

            catch (Exception ex)
            {
                return Result<bool>.InternalServerError("Country set successfully");
            }
        }

        public async  Task<Result<CountryEnum>> GetCountry()
        {
            string userId = _httpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var Country = user.Country;
            return Result<CountryEnum>.Success("", Country);
        }

		public async Task<Result<bool>> SetProfilePic(IFormFile img)
		{
            try
            {
                string name = img.FileName;
				bool isImage = img.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
                if (!isImage)
                    return Result<bool>.BadRequest("Unappropriate data type");

                var user = await _userManager.FindByIdAsync(_httpContext.GetUserId());
                string blobName = "";
                if (string.IsNullOrEmpty(user.ProfilePic)) blobName = user.UserName + Path.GetExtension(img.FileName);
                else blobName = user.ProfilePic;
				var blobClient = _containerClient.GetBlobClient(blobName);
				await blobClient.DeleteIfExistsAsync();
				await blobClient.UploadAsync(img.OpenReadStream(), true);
                user.ProfilePic = blobName;
                await _userManager.UpdateAsync(user);
				return Result<bool>.Success("Profile pic updated successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError("failed to update profile pic");
            }
		}

		public async Task<FileStreamResult> GetProfilePic()
		{
			try
			{
                string fileName = (await _userManager.FindByIdAsync(_httpContext.GetUserId())).ProfilePic;
				var blobClient = _containerClient.GetBlobClient(fileName);
				var memoryStream = new MemoryStream();
				await blobClient.DownloadToAsync(memoryStream);
				memoryStream.Position = 0;
				var contentType = blobClient.GetProperties().Value.ContentType;
				return new FileStreamResult(memoryStream, new MediaTypeHeaderValue("application/octet-stream"))
				{
					FileDownloadName = fileName,
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
                return null;
			}
		}

		public async Task<Result<bool>> DeleteProfilePic()
		{
			try
			{
                string fileName = _httpContext.GetUserId();
				var blobClient = _containerClient.GetBlobClient(fileName);
				await blobClient.DeleteIfExistsAsync();
                return Result<bool>.Success("Deleted pic successfully");

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<bool>.InternalServerError();
			}
		}

        public async Task<Result<bool>> SetFullNameAsync(string firstName, string lastName)
        {
            try
            {
                string userId = _httpContext.GetUserId();
                if (userId == null)
                {
                    return Result<bool>.Unauthorized("User is not valid");
                }
                if (firstName == null)
                {
                    return Result<bool>.BadRequest("firstName is required");
                }

                var user = await _userManager.FindByIdAsync(userId);
                user.FirstName = firstName;
                if (lastName != null)
                {
                    user.LastName = lastName;
                }
                await _userManager.UpdateAsync(user);
                return Result<bool>.Success("First and Second Name is saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError();
            }
        }

        public async Task<Result<FullName>> GetFullNameAsync()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                if (userId == null)
                {
                    return Result<FullName>.Unauthorized("User Id is not valid");
                }

                var user = await _userManager.FindByIdAsync(userId); // Use await to asynchronously get the user
                if (user == null)
                {
                    return Result<FullName>.BadRequest("User Doesn't exist");
                }
                var fullName = new FullName
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                return Result<FullName>.Success("",fullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<FullName>.InternalServerError();
            }


        }
    }

}

