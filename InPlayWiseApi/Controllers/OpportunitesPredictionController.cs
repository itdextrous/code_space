using InPlayWise.Common.Enums;
using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Stripe;

namespace InPlayWiseApi.Controllers
{

    [Authorize]
    [Route("[Controller]")]
    public class OpportunitiesPredictionController : ControllerBase
    {

        private readonly IOpportunitiesPredictionService _predServ;

        public OpportunitiesPredictionController(IOpportunitiesPredictionService predictionService)
        {
            _predServ = predictionService;
        }

        [HttpGet("GetAllOpportunities")]
        public async Task<IActionResult> GetAllOpportunities()
        {
            try
            {
                var result =  _predServ.GetAllOpportunities();
                switch (result.StatusCode)
                {
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

		[HttpGet("GetMatchOpportunities")]
		public async Task<IActionResult> GetAllOpportunities(string matchId)
		{
            return Ok(await _predServ.GetMatchOpportunities(matchId));
		}

        [HttpGet("TrainModelForAll")]
        public async Task<IActionResult> TrainModelForAll()
        {
            return Ok(await _predServ.TrainModelAndUpdateDatabase());
        }

        [HttpGet("GetNumberOfOpportunitiesForMatches")]
        public async Task<IActionResult> GetOpportunitiesCount()
        {
            return Ok(await _predServ.GetOpportunitiesCount());
        }




        [HttpGet("TrainModelWithAllData")]
        public async Task<IActionResult> TrainModelWithAllData()
        {
            return Ok(await _predServ.TrainModelWithAllData());
        }


        //[HttpDelete("dd")]
        //public async Task<IActionResult> DeleteData()
        //{
        //    return Ok(await _predServ.DeleteDailyData());
        //}


        [HttpGet("GetFifteenMatchesWithRecords")]
        public async Task<IActionResult> GetFifteenMatchIdsWithRecords()
        {
            return Ok(await _predServ.GetMatchesIdWithRecord());
        }

        [HttpGet("GetMatchPredictionRecords")]
        public async Task<IActionResult> GetMatchPredictionRecordById(string matchId)
        {
            return Ok(await _predServ.GetPredictionRecordOfMatch(matchId));
        }


        [HttpGet("GetFormattedDataForPrediction")]
        public async Task<IActionResult> GetFormattedDataForPrediction()
        {
            return Ok(await _predServ.GetFormattedDataForPrediction());
        }

        /// <summary>
        /// Retrieves the prediction accuracy for a specific match based on the provided opportunity details.
        /// </summary>
        /// <param name="matchId">The unique identifier of the match for which to retrieve the prediction accuracy.</param>
        /// <param name="type">The type of opportunity (e.g., Over, Under) for which to retrieve the accuracy.</param>
        /// <param name="value">The value associated with the opportunity (e.g., 2.5 goals).</param>
        /// <param name="oppEvent">The event related to the opportunity (e.g., goals, corners).</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the action. The result contains an <see cref="AccuracyDto"/> object
        /// representing the prediction accuracy for the specified match and opportunity details.
        /// </returns>
        /// <response code="200">Returns the prediction accuracy for the specified match and opportunity details.</response>
        /// <response code="400">If any of the provided parameters are invalid or missing.</response>
        /// <response code="500">If an error occurs while retrieving the prediction accuracy.</response>
        [HttpGet("Accuracy/Match")]
        public async Task<IActionResult> GetAccuracyByMatchId(string matchId, OpportunityType type, float value, OpportunityEvent oppEvent)
        {
            try
            {
                var result =  await _predServ.GetAccuracyByMatchIdAsync(matchId, type, value, oppEvent);
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
        /// Retrieves the average prediction accuracy for a specific opportunity based on the provided date and details.
        /// </summary>
        /// <param name="date">The date after which to retrieve the prediction accuracies. The date cannot be in the future.</param>
        /// <param name="type">The type of opportunity (e.g., Over, Under) for which to retrieve the accuracy.</param>
        /// <param name="value">The value associated with the opportunity (e.g., 2.5 goals).</param>
        /// <param name="oppEvent">The event related to the opportunity (e.g., goals, corners).</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the action. The result contains an <see cref="AccuracyDto"/> object
        /// representing the average prediction accuracy for the specified date and opportunity details.
        /// </returns>
        /// <response code="200">Returns the average prediction accuracy for the specified date and opportunity details.</response>
        /// <response code="400">If the specified date is in the future or any of the provided parameters are invalid or missing.</response>
        /// <response code="500">If an error occurs while retrieving the prediction accuracy.</response>
        [HttpGet("Accuracy/ByDate")]
        public async Task<IActionResult> GetAccuracyByDate(DateTime date, OpportunityType type, float value, OpportunityEvent oppEvent)
        {
            try
            {
                var result = await _predServ.GetAccuracyByDate(date, type, value, oppEvent);
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
        /// Retrieves a list of match prediction accuracies for all entries after a specified date.
        /// </summary>
        /// <param name="date">The date after which to retrieve the prediction accuracies. The date cannot be in the future.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the action. The result contains a list of <see cref="MatchAccuracyDto"/> objects
        /// representing the match IDs, opportunities, and accuracies for the specified date.
        /// </returns>
        /// <response code="200">Returns the list of match prediction accuracies for the specified date.</response>
        /// <response code="400">If the specified date is in the future.</response>
        /// <response code="500">If an error occurs while retrieving the prediction accuracies.</response>
        [HttpGet("Accuracy/MatchList")]
        public async Task<IActionResult> GetMatchListAccuracy(DateTime date)
        {
            try
            {
                var result = await _predServ.GetMatchListAccuracy(date);
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
        /// Retrieves the average prediction accuracy for each type of bet opportunity.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the action. The result contains a list of <see cref="AccuracyPerBetDto"/> objects
        /// representing the average accuracy for each type of bet opportunity.
        /// </returns>
        /// <response code="200">Returns the average accuracy for each type of bet opportunity.</response>
        /// <response code="500">If an error occurs while retrieving the prediction accuracies.</response>
        [HttpGet("Accuracy/PerBet")]
        public async Task<IActionResult> GetAccuracyPerBet()
        {
            try
            {
                var result = await _predServ.GetAccuracyPerBet();
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
        /// Retrieves the total average prediction accuracy across all opportunities.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the action. The result contains an <see cref="AccuracyDto"/> object
        /// representing the total average accuracy percentage across all opportunities.
        /// </returns>
        /// <response code="200">Returns the total average accuracy percentage.</response>
        /// <response code="500">If an error occurs while retrieving the prediction accuracies.</response>
        [HttpGet("Accuracy/TotalBet")]
        public async Task<IActionResult> GetTotalAccuracy()
        {
            try
            {
                var result = await _predServ.GetTotalAccuracy();
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
