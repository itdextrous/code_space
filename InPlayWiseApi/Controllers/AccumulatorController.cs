using InPlayWise.Common.DTO;
using InPlayWise.Core.Services;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccumulatorController : ControllerBase
    {
        private readonly IAccumulatorService _accumulatorService;
     
        public AccumulatorController(IAccumulatorService accumulatorService)
        {
            _accumulatorService = accumulatorService;
        }

        /// <summary>
        /// Endpoint to save accumulators.
        /// </summary>
        /// <param name="opportunityDto">List of OpportunityDto objects representing the accumulators to be saved.</param>
        /// <returns>Returns an HTTP OK response with the result of the operation.</returns>
        [HttpPost("SaveAccumulator")]
        public async Task<IActionResult> SaveAccumulatorsAsync([FromBody] List<OpportunityDto> opportunityDtos)
        {
            try
            {
                if (opportunityDtos == null)
                {
                    return BadRequest("Accumulator Can't  be null");
                }

                var result = await _accumulatorService.SaveAccumulatorsAsync                          (opportunityDtos);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
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
        /// Retrieves the saved accumulators from the service asynchronously.
        /// </summary>
        /// <returns>
        /// An IActionResult representing the HTTP response containing the retrieved accumulators,
        /// or an appropriate error response if the operation fails.
        /// </returns>
        [HttpGet("GetAccumulators")]
        public async Task<IActionResult> GetSavedAccumulatorsAsync()
        {
            try
            {
                var result = await _accumulatorService.GetSavedAccumulatorsAsync();
                return StatusCode(result.StatusCode, result);
                //switch (result.StatusCode)
                //{
                //    case 401:
                //        return Unauthorized(result);
                //    case 400:
                //        return BadRequest(result);
                //    case 200:
                //        return Ok(result);
                //    default:
                //        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                //}
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }



        /// <summary>
        /// Endpoint to create hedges.
        /// </summary>
        /// <param name="hedges">List of HedgeDto objects representing the hedges to be created.</param>
        /// <returns>Returns an HTTP OK response with the created hedges.</returns>
        [HttpPost("CreateHedge")]
        public  IActionResult CreateHedges(List<HedgeDto> hedges)
        {
            try
            {
                var result = _accumulatorService.CreateHedges(hedges);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
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
        /// Endpoint to save hedges.
        /// </summary>
        /// <param name="hedges">List of lists of HedgeDto objects representing the hedges to be saved.</param>
        /// <returns>Returns an HTTP OK response with the result of the operation.</returns>
        [HttpPost("SaveHedges")]
        public async Task<IActionResult> SaveHedgesAsync([FromBody] List<List<HedgeDto>> hedges)
        {
            try
            {
                if(hedges.Count == 0)
                {
                    return BadRequest(Result<bool>.BadRequest("Unable to save accumulators"));
                }
                var result = await _accumulatorService.SaveHedgesAsync(hedges);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
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
        /// Endpoint to delete an accumulator by group ID.
        /// </summary>
        /// <param name="groupId">The group ID of the accumulator to be deleted.</param>
        /// <returns>Returns an HTTP OK response with the result of the operation.</returns>
        [HttpDelete("DeleteAccumulator")]
        public async  Task<IActionResult> DeleteAccumulater([FromBody] string groupId)
        {
            try
            {
                var result = await _accumulatorService.DeleteAccumulaterAsync(groupId);
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

    }
}
