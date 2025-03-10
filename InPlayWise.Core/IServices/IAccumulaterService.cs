using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.Services
{
    public interface IAccumulatorService
    {
        /// <summary>
        /// Saves a list of opportunities asynchronously.
        /// </summary>
        /// <param name="opportunityDtos">The list of opportunities to be saved.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a Result object indicating
        /// whether the operation was successful (true) or not (false).
        /// </returns>
        /// <remarks>
        /// This method asynchronously saves the provided list of opportunities to the database.
        /// If the list of opportunities is null, a BadRequest Result with an appropriate error message is returned.
        /// If the user is not authenticated, an Unauthorized Result is returned.
        /// If the operation is successful, the Result object contains a true value.
        /// If the operation fails, the Result object contains a false value along with an error message.
        /// </remarks>
        Task<Result<bool>> SaveAccumulatorsAsync(List<OpportunityDto> opportunityDtos);

        /// <summary>
        /// Retrieves saved accumulators.
        /// </summary>
        /// <returns>Returns a Result containing the list of saved accumulators.</returns>
        Task<Result<List<List<Accumulator>>>> GetSavedAccumulatorsAsync();

        /// <summary>
        /// Creates hedges based on the provided list of HedgeDtos.
        /// </summary>
        /// <param name="hedges">List of HedgeDto objects representing the hedges to be created.</param>
        /// <returns>Returns a Result containing the created hedges.</returns>
        Result<List<List<HedgeResponseDto>>> CreateHedges(List<HedgeDto> hedges);

        /// <summary>
        /// Saves the provided list of hedges.
        /// </summary>
        /// <param name="hedges">List of lists of HedgeDto objects representing the hedges to be saved.</param>
        /// <returns>Returns a Result indicating whether the operation was successful or not.</returns>
        Task<Result<bool>> SaveHedgesAsync(List<List<HedgeDto>> hedges);

        /// <summary>
        /// Deletes an accumulator by group ID.
        /// </summary>
        /// <param name="groupId">The group ID of the accumulator to be deleted.</param>
        /// <returns>Returns a Result indicating whether the operation was successful or not.</returns>
        Task<Result<bool>> DeleteAccumulaterAsync(string groupId);

        Task<bool> UpdateAccumulatorResult(LiveMatchModel liveMatch);

        //Task<bool> FinalAccumulatorResult(LiveMatchModel liveMatch);
    }
}
