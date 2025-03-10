using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace InPlayWise.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AdminRepository> _logger;
        public AdminRepository(AppDbContext db, ILogger<AdminRepository> logger) {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Dictionary<string, List<string>>>> AllUsersUsageAsync()
        {
            try
            {
                // Get all user sessions
                var userSessions = await _db.UserSessions.ToListAsync();

                // Create dictionaries to hold user data
                var userTotalTimes = new Dictionary<string, TimeSpan>();
                var userLoginCounts = new Dictionary<string, int>();

                // Populate user data dictionaries
                foreach (var session in userSessions)
                {
                    var timeSpent = session.DisconnectedTime - session.ConnectedTime;
                    if (!userTotalTimes.ContainsKey(session.UserId))
                    {
                        userTotalTimes[session.UserId] = TimeSpan.Zero;
                    }
                    userTotalTimes[session.UserId] += timeSpent;

                    if (!userLoginCounts.ContainsKey(session.UserId))
                    {
                        userLoginCounts[session.UserId] = 0;
                    }
                    userLoginCounts[session.UserId]++;
                }

                // Create a list to hold result dictionaries
                var resultList = new List<Dictionary<string, List<string>>>();

                // Sort user IDs by total time spent and add to the result list
                var sortedUsersByTimeSpent = userTotalTimes.OrderByDescending(kv => kv.Value)
                                                            .Select(kv => kv.Key)
                                                            .ToList();
                resultList.Add(new Dictionary<string, List<string>>
                {
                    ["Most time spent"] = sortedUsersByTimeSpent
                });

                // Sort user IDs by login count and add to the result list
                var sortedUsersByLoginCount = userLoginCounts.OrderByDescending(kv => kv.Value)
                                                            .Select(kv => kv.Key)
                                                            .ToList();
                resultList.Add(new Dictionary<string, List<string>>
                {
                    ["Most logins"] = sortedUsersByLoginCount
                });

                return resultList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing user sessions.");

                // Return an empty list or any other appropriate response
                return new List<Dictionary<string, List<string>>>();
            }

        }


        public async Task<int> DailyActiveUsersCountAsync()
        {
            try
            {
                // Get the current date in UTC
                DateTime currentDate = DateTime.UtcNow.Date;

                // Query UserSession table for unique user IDs present for the current date
                var userSessionsCount = await _db.UserSessions
                    .Where(session => session.ConnectedTime.Date == currentDate)
                    .Select(session => session.UserId)
                    .Distinct()
                    .CountAsync();

                // Query Accumulators table for unique user IDs with SavedTime equal to current date
                var accumulatorsCount = await _db.Accumulaters
                    .Where(accumulator => accumulator.SavedTime.Date == currentDate)
                    .Select(accumulator => accumulator.UserId)
                    .Distinct()
                    .CountAsync();

                // Find the count of unique user IDs common to both sets
                int commonUserIdsCount = userSessionsCount + accumulatorsCount;

                return commonUserIdsCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching common user IDs count.");
                return -1;
            }
        }

        public Task<int> GetAverageRevenuePerUser()
        {
            int count = -1;
            _logger.LogInformation("Average Revenue per User is not available");
            return Task.FromResult(count);
        }

        public async Task<TimeSpan> GetAverageSessionDurationAsync()
        {
            try
            {
                // Get all user sessions
                var userSessions = await _db.UserSessions.ToListAsync();

                // Calculate total duration of all sessions
                TimeSpan totalDuration = TimeSpan.Zero;
                foreach (var session in userSessions)
                {
                    totalDuration += session.DisconnectedTime - session.ConnectedTime;
                }

                // Calculate average session duration
                TimeSpan averageDuration = totalDuration / userSessions.Count;

                return averageDuration;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating average session duration.");
                return TimeSpan.Zero;
            }
        }

        public async Task<int> GetFreeUsersCountAsync()
        {
            try
            {
                //  count the number of subscriptions where the current subscription is "free"
                int freeUsersCount = await _db.Subscriptions.CountAsync(sub => sub.CurrentSubscription == "free");

                return freeUsersCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user count.");
                return -1;
            }
        }

        public async Task<int> GetGeneratedAccumulatorsCountAsync()
        {
            try
            {
                
                int count = await _db.Accumulaters.Select(acc => acc.GroupId).Distinct().CountAsync();

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the count of generated accumulators.");
                return -1;
            }
        }

        public Task<int> GetAnalyzedStats(Duration duration,int page)
        {
            int count = -1;
            _logger.LogInformation("Hourly Stats is not available");
            return Task.FromResult(count);
        }

        public async Task<List<Dictionary<string, UserLogoutDto>>> GetLastLoggedInTimeForAllUsersAsync(Duration duration)
        {
            try
            {
                DateTime startDate = GetStartDate(duration);

                // Get all user sessions from the database that are within the specified duration
                var userSessions = await _db.UserSessions
                    .Where(session => session.DisconnectedTime >= startDate)
                    .ToListAsync();

                // Get all users from the database
                List<ApplicationUser> users = await _db.Users.ToListAsync();

                // Map each session to a UserLogoutDto and create a dictionary
                var sessionDetails = userSessions.Select(session =>
                {
                    var user = users.FirstOrDefault(u => u.Id.Equals(session.UserId));
                    return new Dictionary<string, UserLogoutDto>
            {
                {
                    session.UserId,
                    new UserLogoutDto
                    {
                        FName = user?.FirstName,
                        LName = user?.LastName,
                        Username = user?.UserName,
                        Email = user?.Email,
                        LastActive = session.DisconnectedTime
                    }
                }
            };
                }).ToList();

                return sessionDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching last logged out times for users.");
                return new List<Dictionary<string, UserLogoutDto>>();
            }
        }

        public async Task<List<UserSession>> GetAllSessionsByTime(DateTime time)
        {
            try
            {
                return await _db.UserSessions.Where(ses => ses.ConnectedTime > time).OrderByDescending(s => s.ConnectedTime).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching last logged out times for users.");
                return null;
            }
        }

        public async Task<int> GetNotificationTracking()
        {
            int count = -1;
            _logger.LogInformation("GetNotificationTracking is not available");
            return -1;
        }

        public async Task<int> GetOpportunitiesCountAsync()
        {
            try
            {
                int OpportuniesCount = await _db.Accumulaters.CountAsync();
                return OpportuniesCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user count.");
                return -1;
               
            }
        }

        public async Task<int> GetRegisteredUsersCountAsync()
        {
            try
            {
                // get the count of users from the database
                int userCount = await _db.Users.CountAsync();
                return userCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the user count.");
                return -1;
            }
        }

        public async Task<int> GetTicketAndQueries()
        {
            int count = -1;
            _logger.LogInformation("GetTicketAndQueries is not available");
            return -1;
        }

        public async Task<int> GetUsersCountPerCountryAsync(CountryEnum country)
        {
            try
            {
                // count the number of users in the specified country
                int count = await _db.Users.CountAsync(u => u.Country == country);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while counting users by country.");
                return -1;
            }
        }

        public async Task<int> PlanDowngradesCountAsync()
        {
            try
            {
                int count = await _db.Subscriptions.CountAsync(sub => sub.IsDowngrade);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while counting subscriptions with IsDowngrade.");
                    return -1;
            }
        }

        public async Task<int> UpgradesCountAsync()
        {
            try
            {
                int count = await _db.Subscriptions.CountAsync(sub => sub.IsUpgrade);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while counting subscriptions with IsDowngrade.");
                    return -1;
            }
        }

        public async Task<List<Dictionary<Hour, int>>> GetHourlyStats()
        {
            try
            {
                var currentHour = (Hour)DateTime.UtcNow.Hour;
                var startTime = DateTime.UtcNow.AddHours(-((int)currentHour + 24));

                var latestSessions = await _db.UserSessions
                                               .Where(s => s.ConnectedTime >= startTime)
                                               .OrderByDescending(s => s.ConnectedTime)
                                               .ToListAsync();

                var hourlyStats = new Dictionary<Hour, int>();
                foreach (Hour hour in Enum.GetValues(typeof(Hour)))
                {
                    hourlyStats[hour] = 0;
                }

                foreach (var session in latestSessions)
                {
                    var hourOfDay = (Hour)session.ConnectedTime.Hour;
                    hourlyStats[hourOfDay]++;
                }

                var result = new List<Dictionary<Hour, int>>();
                result.Add(hourlyStats);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetHourlyStats: {ex}");
                throw;
            }
        }

        public async Task<List<Dictionary<Hour, int>>> GetWeeklyStats()
        {
            try
            {
                var currentHour = (Hour)DateTime.UtcNow.Hour;
                var startTime = DateTime.UtcNow.AddHours(-((int)currentHour + (24*7)));

                var latestSessions = await _db.UserSessions
                                               .Where(s => s.ConnectedTime >= startTime)
                                               .OrderByDescending(s => s.ConnectedTime)
                                               .ToListAsync();

                var hourlyStats = new Dictionary<Hour, int>();
                foreach (Hour hour in Enum.GetValues(typeof(Hour)))
                {
                    hourlyStats[hour] = 0;
                }

                foreach (var session in latestSessions)
                {
                    var hourOfDay = (Hour)session.ConnectedTime.Hour;
                    hourlyStats[hourOfDay]++;
                }

                var result = new List<Dictionary<Hour, int>>();
                result.Add(hourlyStats);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetHourlyStats: {ex}");
                throw;
            }
        }

        public async Task<List<Dictionary<Hour, int>>> GetMonthlyStats()
        {
            try
            {
                var currentHour = (Hour)DateTime.UtcNow.Hour;
                var startTime = DateTime.UtcNow.AddHours(-((int)currentHour + (24 * 7)));

                var latestSessions = await _db.UserSessions
                                               .Where(s => s.ConnectedTime >= startTime)
                                               .OrderByDescending(s => s.ConnectedTime)
                                               .ToListAsync();

                var hourlyStats = new Dictionary<Hour, int>();
                foreach (Hour hour in Enum.GetValues(typeof(Hour)))
                {
                    hourlyStats[hour] = 0;
                }

                foreach (var session in latestSessions)
                {
                    var hourOfDay = (Hour)session.ConnectedTime.Hour;
                    hourlyStats[hourOfDay]++;
                }

                var result = new List<Dictionary<Hour, int>>();
                result.Add(hourlyStats);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetHourlyStats: {ex}");
                throw;
            }
        }

        public async Task<List<Dictionary<Hour, int>>> GetQuarterlyStats()
        {
            try
            {
                var currentHour = (Hour)DateTime.UtcNow.Hour;
                var startTime = DateTime.UtcNow.AddHours(-((int)currentHour + (24 * 31 * 3)));

                var latestSessions = await _db.UserSessions
                                               .Where(s => s.ConnectedTime >= startTime)
                                               .OrderByDescending(s => s.ConnectedTime)
                                               .ToListAsync();

                var hourlyStats = new Dictionary<Hour, int>();
                foreach (Hour hour in Enum.GetValues(typeof(Hour)))
                {
                    hourlyStats[hour] = 0;
                }

                foreach (var session in latestSessions)
                {
                    var hourOfDay = (Hour)session.ConnectedTime.Hour;
                    hourlyStats[hourOfDay]++;
                }

                var result = new List<Dictionary<Hour, int>>();
                result.Add(hourlyStats);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetHourlyStats: {ex}");
                throw;
            }
        }

        public async Task<List<Dictionary<Hour, int>>> GetYearlyStats()
        {
            try
            {
                var currentHour = (Hour)DateTime.UtcNow.Hour;
                var startTime = DateTime.UtcNow.AddHours(-((int)currentHour + (24 * 365)));

                var latestSessions = await _db.UserSessions
                                               .Where(s => s.ConnectedTime >= startTime)
                                               .OrderByDescending(s => s.ConnectedTime)
                                               .ToListAsync();

                var hourlyStats = new Dictionary<Hour, int>();
                foreach (Hour hour in Enum.GetValues(typeof(Hour)))
                {
                    hourlyStats[hour] = 0;
                }

                foreach (var session in latestSessions)
                {
                    var hourOfDay = (Hour)session.ConnectedTime.Hour;
                    hourlyStats[hourOfDay]++;
                }

                var result = new List<Dictionary<Hour, int>>();
                result.Add(hourlyStats);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetHourlyStats: {ex}");
                throw;
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

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                return await _db.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<Dictionary<string, int>> GetAllSubscriptionsCount()
        {
            try
            {
                return await _db.Subscriptions
                    .GroupBy(subscription => subscription.ProductId)
                    .Select(group => new
                    {
                        ProductId = group.Key,
                        Count = group.Count()
                    })
                    .ToDictionaryAsync(x => x.ProductId.ToString(), x => x.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<LoginHistory>> GetLoginHistoryByTime(DateTime time)
        {
            try
            {
                return await _db.LoginHistory.Where(rec => rec.LoginTime > time).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            try
            {
                return await _db.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
