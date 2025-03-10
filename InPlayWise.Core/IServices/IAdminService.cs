using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Data.Entities;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InPlayWise.Core.IServices
{
    public interface IAdminService
    {
        public Result<string> GetStripeBalance();

        /// <summary>
        /// Retrieves the count of registered users asynchronously.
        /// </summary>
        /// <returns>A Result object containing the count of registered users.</returns>
        Task<Result<int>> GetRegisteredUsersCountAsync();
         ///<summary>
        /// Retrieves the count of users per subscription plan asynchronously.
        /// </summary>
        /// <returns>
        /// A Result object containing a dictionary with the count of users per subscription plan.
        /// Returns a NotFound result if no users are found for any plan.
        /// Returns an InternalServerError result if an exception occurs.
        /// </returns>
        Task<Result<Dictionary<string, int>>> GetUsersPerPlanCountAsync();
        /// <summary>
        /// Retrieves the count of opportunities asynchronously.
        /// </summary>
        /// <returns>
        /// A Result object containing the count of opportunities.
        /// Returns a NotFound result if no opportunities are found.
        /// Returns an InternalServerError result if an exception occurs.
        /// </returns>
        Task<Result<int>> GetOpportunitiesCountAsync();
        /// <summary>
        /// Retrieves the count of users per specified country asynchronously.
        /// </summary>
        /// <param name="country">The country for which the user count is to be retrieved.</param>
        /// <returns>
        /// A Result object containing the count of users for the specified country.
        /// Returns a NotFound result if no users are found for the given country.
        /// Returns an InternalServerError result if an exception occurs.
        /// </returns>
        Task<Result<int>> GetUsersCountPerCountryAsync(CountryEnum country);
        /// <summary>
        /// Retrieves the count of free users asynchronously.
        /// </summary>
        /// <returns>
        /// A Result object containing the count of free users.
        /// Returns a NotFound result if no free users exist.
        /// Returns an InternalServerError result if an exception occurs.
        /// </returns>
        Task<Result<int>> GetFreeUsersCountAsync();
        /// <summary>
        /// Retrieves the count of current active users asynchronously.
        /// </summary>
        /// <returns>
        /// A Result object containing the count of current active users.
        /// Returns an InternalServerError result if an exception occurs.
        /// </returns>
        Task<Result<int>> GetCurrentActiveUsersCountAsync();
        /// <summary>
        /// Retrieves the count of current active users per plan asynchronously.
        /// </summary>
        /// <returns>
        /// A Result object containing a dictionary with the count of current active users per plan.
        /// Returns a NotFound result if no active users are found.
        /// Returns an InternalServerError result if an exception occurs.
        /// </returns>
        Task<Result<Dictionary<string, int>>> GetCurrentUsersCountPerPlan();
        /// <summary>
        /// Retrieves the count of daily active users asynchronously and returns a Result object.
        /// </summary>
        /// <returns>
        /// A Result object containing the count of daily active users.
        /// Returns a NotFound result if no daily active users are found.
        /// Returns an InternalServerError result if an exception occurs during processing.
        /// </returns>
        Task<Result<int>> DailyActiveUsersCountAsync();
        /// <summary>
        /// Retrieves usage statistics for all users asynchronously and returns a Result object.
        /// </summary>
        /// <returns>
        /// A Result object containing a list of dictionaries with usage statistics for all users.
        /// Returns a NotFound result if no usage statistics are found.
        /// Returns an InternalServerError result if an exception occurs during processing.
        /// </returns>
        Task<Result<List<Dictionary<String, List<string>>>>> AllUsersUsageAsync();
        /// <summary>
        /// Retrieves the count of subscriptions with upgrades asynchronously and returns a Result object.
        /// </summary>
        /// <returns>
        /// A Result object containing the count of subscriptions with upgrades.
        /// Returns a NotFound result if no upgrades are found.
        /// Returns an InternalServerError result if an exception occurs during processing.
        /// </returns>
        Task<Result<int>> UpgradesCountAsync();

        /// <summary>
        /// Retrieves the count of subscriptions with downgrades asynchronously and returns a Result object.
        /// </summary>
        /// <returns>
        /// A Result object containing the count of subscriptions with downgrades.
        /// Returns a NotFound result if no downgrades are found.
        /// Returns an InternalServerError result if an exception occurs during processing.
        /// </returns>
        Task<Result<int>> PlanDowngradesCountAsync();
        /// <summary>
        /// Retrieves the average duration of user sessions asynchronously and returns a Result object.
        /// </summary>
        /// <returns>
        /// A Result object containing the average duration of user sessions.
        /// Returns a NotFound result if the average duration is not found.
        /// Returns an InternalServerError result if an exception occurs during processing.
        /// </returns>
        Task<Result<TimeSpan>> GetAverageSessionDurationAsync();
      
        Task<Result<int>> GetAverageRevenuePerUserAsync();

        /// <summary>
        /// Retrieves the last logged-in time for all users asynchronously and returns a Result object.
        /// </summary>
        /// <returns>
        /// A Result object containing a list of dictionaries with user IDs and their last logged-in times.
        /// Returns a Success result with the data if retrieval is successful.
        /// Returns an InternalServerError result if an exception occurs during processing.
        /// </returns>
        Task<Result<List<Dictionary<string, UserLogoutDto>>>> GetLastLoggedInTimeForAllUsersAsync(Duration duration);

        Task<Result<List<UserSessionRecordDto>>> GetAllUserSessions(DateTime time);


        /// <summary>
        /// Retrieves the count of generated accumulators and returns a Result object.
        /// </summary>
        /// <returns>
        /// A Result object containing the count of generated accumulators.
        /// Returns a NotFound result with a message if no accumulators are found.
        /// Returns a Success result with the count if retrieval is successful.
        /// Returns an InternalServerError result if an exception occurs during processing.
        /// </returns>
        Task<Result<int>> GetGeneratedAccumulatorsCountAsync();
        Task<Result<List<Dictionary<Hour,int>>>> GetHourlyStatsAsync(Duration duration);
        Task<Result<int>> GetTicketAndQueriesAsync();
        Task<Result<int>> GetNotificationTrackingAsync();
    }
}
