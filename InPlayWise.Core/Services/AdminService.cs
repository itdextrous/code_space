using System.Collections.Generic;
using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IStripePaymentService _stripePaymentService;
        private readonly ILogger<AdminService> _logger;
        private readonly MatchInMemoryService _inMemory;
        public AdminService(IAdminRepository adminRepo, IStripePaymentService stripePaymentService, ILogger<AdminService> logger, MatchInMemoryService inMemory)
        {
            _adminRepo = adminRepo;
            _stripePaymentService = stripePaymentService;
            _logger = logger;
            _inMemory = inMemory;
        }

        public Result<string> GetStripeBalance()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<int>> GetRegisteredUsersCountAsync()
        {
            try
            {
                int count= await _adminRepo.GetRegisteredUsersCountAsync();
                if (count == null)
                {
                    return Result<int>.NotFound("There is no registered user");
                }
                return Result<int>.Success("",count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }
        public async Task<Result<Dictionary<string, int>>> GetUsersPerPlanCountAsync()
        {
            try
            {
                Dictionary<string, int> allSubscriptionsCount = await _adminRepo.GetAllSubscriptionsCount();
                List<Product> products = await _adminRepo.GetAllProducts();
                Dictionary<string, int> result = new Dictionary<string, int>();
                foreach (Product product in products) {
                    if (allSubscriptionsCount.ContainsKey(product.Id.ToString()))
                    {
                        result[product.Name] = allSubscriptionsCount[product.Id.ToString()]; 
                    }
                    else
                    {
                        result[product.Name] = 0;
                    }
                }
                return Result<Dictionary<string, int>>.Success(string.Empty, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user counts per plan.");
                return Result<Dictionary<string, int>>.InternalServerError("An error occurred while fetching user counts per plan.");
            }
        }
        public async Task<Result<int>> GetOpportunitiesCountAsync()
        {            
            try
            {
               int oppCount= await  _adminRepo.GetOpportunitiesCountAsync();
                if (oppCount == 0)
                {
                    return Result<int>.NotFound("There is No Opportunities");
                }
                return Result<int>.Success("", oppCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError();
            }
        }

        public async Task<Result<int>> GetUsersCountPerCountryAsync(CountryEnum country)
        {
            try
            {
                int count = await _adminRepo.GetUsersCountPerCountryAsync(country);
                if (count == 0)
                {
                    return Result<int>.NotFound("No user is present for Given Country");
                }
                return Result<int>.Success("",count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError($"Failed to get users count per country: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetFreeUsersCountAsync()
        {
            try
            {
                //  get the count of free users from the repository
                int count = await _adminRepo.GetFreeUsersCountAsync();
                if(count == 0)
                {
                    return Result<int>.NotFound("No Free User Exist");
                }
                return Result<int>.Success("", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<int>> GetCurrentActiveUsersCountAsync()
        {
            try
            {
				int res =  _inMemory.GetUserSessions().Count;
                if (res == 0)
                {
                    return Result<int>.NotFound("No There is No Active User Session Currently");
                }
				return Result<int>.Success("", res);
			}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<Dictionary<string, int>>> GetCurrentUsersCountPerPlan()
        {
            try
            {
                // Get the list of active users
                List<UserSession> activeUsers = _inMemory.GetUserSessions();
                List<Product> allProducts = await _adminRepo.GetAllProducts();
                Dictionary<string, int> result = new Dictionary<string, int>();
                foreach (Product product in allProducts) {
                    result[product.Name] = activeUsers.Where(user => user.ProductId.Equals(product.Id.ToString())).Count();
                }

                return Result<Dictionary<string, int>>.Success("", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<Dictionary<string, int>>.InternalServerError();
            }
        }

        public async Task<Result<int>> DailyActiveUsersCountAsync()
        {
            try
            {
                int res = await _adminRepo.DailyActiveUsersCountAsync();
                if(res==0)
                {
                    return Result<int>.NotFound("There is no Daily Active User");
                }
                return Result<int>.Success("", res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError();
            }
        }

        public async Task<Result<List<Dictionary<String, List<string>>>>> AllUsersUsageAsync()
        {
            try
            {
                var usageStatistics = await _adminRepo.AllUsersUsageAsync();

                // Check if the result is null
                if (usageStatistics == null)
                {
                    return Result<List<Dictionary<string, List<string>>>>.NotFound("No usage statistics found.");
                }
                return Result<List<Dictionary<String, List<string>>>>.Success("", usageStatistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<Dictionary<String,List<string>>>>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<int>> UpgradesCountAsync()
        {
            try
            {
                int count = await _adminRepo.UpgradesCountAsync();
                if (count == 0)
                {
                    return Result<int>.NotFound("There is no Upgades");
                }
                return Result<int>.Success("", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<int>> PlanDowngradesCountAsync()
        {
            try
            {
                int count = await _adminRepo.PlanDowngradesCountAsync();
                if (count == 0)
                {
                    return Result<int>.NotFound("There is no downgades");
                }
                return Result<int>.Success("", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<TimeSpan>> GetAverageSessionDurationAsync()
        {
            try
            {
               TimeSpan Duration = await _adminRepo.GetAverageSessionDurationAsync();
                return Result<TimeSpan>.Success("", Duration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<TimeSpan>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<int>> GetAverageRevenuePerUserAsync()
        {
            try
            {
                int count = await _adminRepo.GetAverageRevenuePerUser();
                return Result<int>.Success("", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<List<Dictionary<string, UserLogoutDto>>>> GetLastLoggedInTimeForAllUsersAsync(Duration duration)
        {
            try
            {

                var lastLogins = await _adminRepo.GetLastLoggedInTimeForAllUsersAsync(duration);
                List<LoginHistory> loginHistories = await _adminRepo.GetLoginHistoryByTime(GetStartDate(duration));
                List<ApplicationUser> users = await _adminRepo.GetAllUsers();
                List<Dictionary<string, UserLogoutDto>> result = new List<Dictionary<string, UserLogoutDto>>();
                foreach (LoginHistory hist in loginHistories)
                {
                    Dictionary<string, UserLogoutDto> entry = new Dictionary<string, UserLogoutDto>();
                    ApplicationUser user = users.FirstOrDefault(user => user.Id.Equals(hist.UserId));
                    if(user is null) { continue; }
                    entry[hist.UserId] = new UserLogoutDto()
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        FName = user.FirstName,
                        LName = user.LastName,
                        LastActive = hist.LoginTime
                    };
                    result.Add(entry);
                }

                if (lastLogins == null)
                {
                    return Result<List<Dictionary<string, UserLogoutDto>>>.NotFound("No user is availible with last login time");
                }
                return Result<List<Dictionary<string, UserLogoutDto>>>.Success("", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<Dictionary<string, UserLogoutDto>>>.InternalServerError(ex.Message);
            }
        }


        public async Task<Result<List<UserSessionRecordDto>>> GetAllUserSessions(DateTime time)
        {
            try
            {
                List<UserSession> allSessions = await _adminRepo.GetAllSessionsByTime(time);
                List<UserSessionRecordDto> result = new List<UserSessionRecordDto>();
                Dictionary<string, List<UserSession>> userIdToSessionsMap = allSessions
                    .GroupBy(session => session.UserId)
                    .ToDictionary(
                        group => group.Key,       // Key: userId
                        group => group.ToList()   // Value: List of UserSession for that userId
                    );
                List<ApplicationUser> users = await _adminRepo.GetAllUsers();
                List<LoginHistory> loginHistory = await _adminRepo.GetLoginHistoryByTime(time);
                foreach (string userId in userIdToSessionsMap.Keys)
                {
                    ApplicationUser user = users.FirstOrDefault(user => user.Id.Equals(userId));
                    if (user is null) continue;
                    UserSessionRecordDto record = new UserSessionRecordDto
                    {
                        UserId = userId,
                        Email = user.Email,
                        FName = user.FirstName,
                        LName = user.LastName,
                        Username = user.UserName,
                        sessions = userIdToSessionsMap[userId].Select(ses => new SessionTimeDto
                            {
                                ConnectedTime = ses.ConnectedTime,
                                DisconnectedTime = ses.DisconnectedTime
                            }).ToList(),
                        LoginCount = loginHistory.Where(lh => lh.UserId.Equals(userId)).ToList().Count(),
                        ActiveMinutes = userIdToSessionsMap[userId]
                            .Sum(session => (int)(session.DisconnectedTime - session.ConnectedTime).TotalMinutes)
                    };
                    result.Add(record);
                }
                return Result<List<UserSessionRecordDto>>.Success("", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<UserSessionRecordDto>>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<int>> GetTicketAndQueriesAsync()
        {
            try
            {
                int count = await _adminRepo.GetTicketAndQueries();
                return Result<int>.Success("", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<int>> GetNotificationTrackingAsync()
        {
            try
            {
                int count = await _adminRepo.GetNotificationTracking();
                return Result<int>.Success("", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }

        public async Task<Result<int>> GetGeneratedAccumulatorsCountAsync()
        {
            try
            {
                int count = await _adminRepo.GetGeneratedAccumulatorsCountAsync();
                if (count==0)
                {
                    return Result<int>.NotFound("No Accumulators found");
                }
                return Result<int>.Success("", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<int>.InternalServerError(ex.Message);
            }
        }

        public async  Task<Result<List<Dictionary<Hour,int>>>> GetHourlyStatsAsync(Duration duration)
        {
            try
            {
                List<Dictionary<Hour, int>> count = new List<Dictionary<Hour, int>>();

                switch (duration)
                {
                    case Duration.Hourly:
                        count = await _adminRepo.GetHourlyStats();
                        break;
                    case Duration.Weekly:
                        count = await _adminRepo.GetWeeklyStats();
                        break;
                    case Duration.Monthly:
                        count = await _adminRepo.GetMonthlyStats();
                        break;
                    case Duration.Quarterly:
                        count = await _adminRepo.GetQuarterlyStats();
                        break;
                    case Duration.Yearly:
                        count = await _adminRepo.GetYearlyStats();
                        break;
                    case Duration.Unknown:
                    default:
                       
                        break;
                }
                return Result<List<Dictionary<Hour,int>>>.Success("",count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<Dictionary<Hour,int>>>.InternalServerError(ex.Message);
            }
        }

        private DateTime GetStartDate(Duration duration)
        {
            DateTime now = DateTime.Now;

            switch (duration)
            {
                case Duration.Hourly:
                    return now.AddHours(-1);
                case Duration.Weekly:
                    return now.AddDays(-7);
                case Duration.Monthly:
                    return now.AddMonths(-1);
                case Duration.Quarterly:
                    return now.AddMonths(-3);
                case Duration.Yearly:
                    return now.AddYears(-1);
                case Duration.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(duration), duration, null);
            }
        }


    }
}
