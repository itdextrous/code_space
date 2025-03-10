using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.MembershipEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IAdminRepository
    {
        /// <summary>
        /// Retrieves the count of registered users asynchronously.
        /// </summary>
        /// <returns>The count of registered users, or -1 if an error occurs.</returns>
        Task<int> GetRegisteredUsersCountAsync();

        /// <summary>
        /// Retrieves the count of opportunities asynchronously.
        /// </summary>
        /// <returns>The count of opportunities, or -1 if an error occurs.</returns>
        Task<int> GetOpportunitiesCountAsync();
        /// <summary>
        /// Retrieves the count of users for the specified country asynchronously.
        /// </summary>
        /// <param name="country">The country for which the user count is to be retrieved.</param>
        /// <returns>The count of users for the specified country, or -1 if an error occurs.</returns>
        Task<int> GetUsersCountPerCountryAsync(CountryEnum country);
        // <summary>
        /// Retrieves the count of free users asynchronously.
        /// </summary>
        /// <returns>The count of free users, or -1 if an error occurs.</returns>
        Task<int> GetFreeUsersCountAsync();
        // <summary>
        /// Retrieves the count of daily active users asynchronously.
        /// </summary>
        /// <returns>
        /// An integer representing the count of daily active users.
        /// Returns -1 if an error occurs during processing.
        /// </returns>
        Task<int> DailyActiveUsersCountAsync();
        // <summary>
        /// Retrieves usage statistics for all users asynchronously.
        /// </summary>
        /// <returns>
        /// A list of dictionaries containing usage statistics for all users, sorted by most time spent and most logins.
        /// Each dictionary includes keys "Most time spent" and "Most logins" with corresponding user IDs.
        /// Returns an empty list if an error occurs during processing.
        /// </returns>
        Task<List<Dictionary<String, List<string>>>> AllUsersUsageAsync();
        /// <summary>
        /// Retrieves the count of subscriptions with the IsUpgrade flag set to true asynchronously.
        /// </summary>
        /// <returns>
        /// An integer representing the count of subscriptions with upgrades.
        /// Returns -1 if an error occurs during processing.
        /// </returns>
        Task<int> UpgradesCountAsync();
        /// <summary>
        /// Retrieves the count of subscriptions with the IsDowngrade flag set to true asynchronously.
        /// </summary>
        /// <returns>
        /// An integer representing the count of subscriptions with downgrades.
        /// Returns -1 if an error occurs during processing.
        /// </returns>
        Task<int> PlanDowngradesCountAsync();
        /// <summary>
        /// Retrieves the average duration of user sessions asynchronously.
        /// </summary>
        /// <returns>
        /// A TimeSpan representing the average duration of user sessions.
        /// Returns TimeSpan.Zero if an error occurs during processing.
        Task<TimeSpan> GetAverageSessionDurationAsync();

        Task<int> GetAverageRevenuePerUser();
        /// <summary>
        /// Retrieves the last logged-in time for all users asynchronously.
        /// </summary>
        /// <returns>
        /// A list of dictionaries containing the user ID as key and the last logged-in time as value.
        /// Returns an empty list if an error occurs during processing.
        /// </returns>
        Task<List<Dictionary<string, UserLogoutDto>>> GetLastLoggedInTimeForAllUsersAsync(Duration duration);
        Task<List<UserSession>> GetAllSessionsByTime(DateTime time);
        Task<int> GetTicketAndQueries();
        Task<int> GetNotificationTracking();
        // <summary>
        /// Retrieves the count of Accumulators in Accumulater table asynchronously.
        /// </summary>
        /// <returns>
        /// An integer representing the count  Accumulators.
        /// Returns -1 if an error occurs during processing.
        Task<int> GetGeneratedAccumulatorsCountAsync();
        Task<List<Dictionary<Hour,int>>> GetHourlyStats();
        Task<List<Dictionary<Hour, int>>> GetWeeklyStats();
        Task<List<Dictionary<Hour, int>>> GetMonthlyStats();
        Task<List<Dictionary<Hour, int>>> GetQuarterlyStats();
        Task<List<Dictionary<Hour, int>>> GetYearlyStats();

        Task<List<Product>> GetAllProducts();
        Task<Dictionary<string, int>> GetAllSubscriptionsCount();

        Task<List<LoginHistory>> GetLoginHistoryByTime(DateTime time);
        Task<List<ApplicationUser>> GetAllUsers();

    }
}
