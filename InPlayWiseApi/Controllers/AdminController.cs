using InPlayWise.Common.Enums;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InPlayWiseApi.Controllers
{
    /// <summary>
    /// Controller for handling administrative tasks and retrieving various statistics.
    /// </summary>
    [Route("[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {

        private readonly IAdminService _adminService;
       
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Retrieves the count of registered users.
        /// </summary>
        [HttpGet("GetRegistredUsersCount")]
        public async Task<IActionResult> GetRegisteredUsersCountAsync()
        {
            try
            {
                var result = await _adminService.GetRegisteredUsersCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        /// <summary>
        /// Retrieves the count of users per plan.
        /// </summary>
        [HttpGet("GetUsersPerPlanCount")]
        public async Task<IActionResult> GetUsersPerPlanCountAsync()
        {
            try
            {
                var result = await _adminService.GetUsersPerPlanCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the count of opportunities saved.
        /// </summary>
        [HttpGet("GetCountOfOpportunitiesSaved")]
        public async Task<IActionResult> GetOpportunitiesCountAsync()
        {
            try
            {
                var result = await _adminService.GetOpportunitiesCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the count of users for the specified country asynchronously.
        /// </summary>
        /// <param name="country">The country for which the user count is to be retrieved.</param>
        /// <returns>
        /// A JSON response containing the count of users for the specified country.
        /// Returns HTTP 200 (OK) with the user count if successful.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("GetUserCountForCountry")]
        public async Task<IActionResult> GetUsersCountPerCountryAsync(CountryEnum country)
        {
            try
            {

                if (!Enum.IsDefined(typeof(CountryEnum), country))
                {
                    return BadRequest("Invalid country specified.");
                }
                var result = await _adminService.GetRegisteredUsersCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the count of free users.
        /// </summary>
        [HttpGet("GetFreeUsersCount")]
        public async Task<IActionResult> GetFreeUsersCountAsync()
        {
            try
            {
                var result = await _adminService.GetFreeUsersCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the count of current active users asynchronously.
        /// </summary>
        /// <returns>
        /// An HTTP response containing the count of current active users.
        /// Returns HTTP 200 (OK) with the count of current active users if successful.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("GetCurrentActiveUsersCount")]
        public async Task<IActionResult> GetCurrentActiveUsersCountAsync()
        {
            try
            {
                var result = await _adminService.GetCurrentActiveUsersCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the count of current active users per plan asynchronously.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a Result object with a dictionary representing the count of current active users per plan.
        /// Returns HTTP 200 (OK) with the Result object if successful.
        /// Returns HTTP 404 (Not Found) if no active users are found.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("GetCurrentUsersCountPerPlan")]
        public async Task<IActionResult> GetCurrentUsersCountPerPlanAsync()
        {
            try
            {
                var result = await _adminService.GetCurrentUsersCountPerPlan();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the count of daily active users asynchronously.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a Result object with the count of daily active users.
        /// Returns HTTP 200 (OK) with the Result object if successful.
        /// Returns HTTP 404 (Not Found) if no daily active users are found.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("DailyActiveUsersCount")]
        public async Task<IActionResult> DailyActiveUsersCountAsync()
        {
            try
            {
                var result = await _adminService.DailyActiveUsersCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves usage statistics for all users and returns an HTTP response.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a Result object with usage statistics for all users.
        /// Returns HTTP 200 (OK) with the Result object if successful.
        /// Returns HTTP 404 (Not Found) if no usage statistics are found.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("AllUsersUsageStats")]
        public async Task<IActionResult> AllUsersUsageAsync()
        {
            try
            {
                var result = await _adminService.AllUsersUsageAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the count of subscriptions with upgrades and returns an HTTP response.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a Result object with the count of subscriptions with upgrades.
        /// Returns HTTP 200 (OK) with the Result object if successful.
        /// Returns HTTP 404 (Not Found) if no upgrades are found.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("PlanUpgradesCount")]
        public async Task<IActionResult> UpgradesCountAsync()
        {
            try
            {
                var result = await _adminService.UpgradesCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the count of subscriptions with downgrades and returns an HTTP response.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a Result object with the count of subscriptions with downgrades.
        /// Returns HTTP 200 (OK) with the Result object if successful.
        /// Returns HTTP 404 (Not Found) if no downgrades are found.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("PlanDowngradesCount")]
        public async Task<IActionResult> PlanDowngradesCountAsync()
        {
            try
            {
                var result = await _adminService.PlanDowngradesCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves the average duration of user sessions and returns an HTTP response.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a Result object with the average duration of user sessions.
        /// Returns HTTP 200 (OK) with the Result object if successful.
        /// Returns HTTP 404 (Not Found) if the average duration is not found.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("GetAverageSessionDuration")]
        public async Task<IActionResult> GetAverageSessionDurationAsync()
        {
            try
            {
                var result = await _adminService.GetAverageSessionDurationAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpGet("GetAverageRevenuePerUser")]
        public async Task<IActionResult> GetAverageRevenuePerUserAsync()
        {
            return Ok(await _adminService.GetAverageRevenuePerUserAsync());
        }

        /// <summary>
        /// Retrieves the last logged-in time for all users and returns an HTTP response.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a Result object with the last logged-in times for all users.
        /// Returns HTTP 200 (OK) with the Result object if successful.
        /// Returns HTTP 404 (Not Found) if the last logged-in times are not found.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("GetLastLoggedInTimeForAllUsers")]
        public async Task<IActionResult> GetLastLoggedInTimeForAllUsersAsync(Duration duration)
        {
            try
            {
                if (duration == Duration.Unknown) { return BadRequest("Unexpected duration requested"); }
                var result = await _adminService.GetLastLoggedInTimeForAllUsersAsync(duration);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("GetAllSessions")]
        public async Task<IActionResult> GetAllSessions(DateTime time)
        {
            try
            {
                var result = await _adminService.GetAllUserSessions(time);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("GetTicketAndQueries")]
        public async Task<IActionResult> GetTicketAndQueriesAsync()
        {
            try
            {
                var result = await _adminService.GetTicketAndQueriesAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetNotificationTracking")]
        public async Task<IActionResult> GetNotificationTrackingAsync()
        {
            try
            {
                var result = await _adminService.GetNotificationTrackingAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Retrieves the count of generated accumulators and returns an HTTP response.
        /// </summary>
        /// <returns>
        /// An HTTP response containing a Result object with the count of generated accumulators.
        /// Returns HTTP 200 (OK) with the Result object if successful.
        /// Returns HTTP 404 (Not Found) if no accumulators are found.
        /// Returns HTTP 500 (Internal Server Error) if an exception occurs during processing.
        /// </returns>
        [HttpGet("GetGeneratedAccumulatorsCount")]
        public async Task<IActionResult> GetGeneratedAccumulatorsCountAsync()
        {
            try
            {
                var result = await _adminService.GetGeneratedAccumulatorsCountAsync();
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetHourlyStats")]
        public async Task<IActionResult> GetHourlyStatsAsync(Duration duration)
        {
            try
            {
                var result = await _adminService.GetHourlyStatsAsync(duration);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }   
}