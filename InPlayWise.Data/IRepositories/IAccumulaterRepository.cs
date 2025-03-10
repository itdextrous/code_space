using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Data.IRepositories
{
    public interface IAccumulaterRepository
    {
        /// <summary>
        /// Asynchronously saves a list of accumulators to the database.
        /// </summary>
        /// <param name="accumulators">The list of accumulators to save.</param>
        /// <returns>
        /// True if the saving operation is successful; otherwise, false.
        /// </returns>
        Task<bool> SaveAccumulatorsAsync(List<Accumulator> accumulaters);

        /// <summary>
        /// Retrieves saved accumulaters based on the provided user ID and group them based on groupId.
        /// </summary>
        /// <param name="UserId">The user ID for which to retrieve the saved accumulaters.</param>
        /// <returns>Returns a list of lists of Accumulater objects representing the saved accumulaters.</returns>
        Task<List<List<Accumulator>>> GetSavedAccumulatorsAsync(string UserId);

       
       

        /// <summary>
        /// Deletes an accumulator by group ID from database.
        /// </summary>
        /// <param name="groupId">The group ID of the accumulator to be deleted.</param>
        /// <returns>Returns a boolean indicating whether the operation was successful or not.</returns>
        Task<bool> DeleteAccumulatorAsync(string groupId);
        /// <summary>
        /// Retrieves no. of Accumulators to retrieved by user based on the producId.
        /// </summary>
        /// <param name="productId">The group ID of the accumulator to be deleted.</param>
        /// <returns>Returns a integer indicating No.of Accumulators allowed to retrieve.</returns>
        Task<int> GetAccumulatorHistoryLimit(string productId);

        Task<int> GetGeneratedAccumulatorLimit(string productId);
        Task<List<Accumulator>> GetAllOpps(string matchId);

        Task<bool> UpdateAccumulator(List<Accumulator> accumulaters);

        /// <summary>
        /// Checks if accumulators exist in the database for the given list of opportunity data transfer objects (DTOs) 
        /// and user ID. For each opportunity, the method verifies that accumulators exist with the same MatchId, 
        /// OpportunityName, and UserId, and that all matching accumulators share the same GroupId.
        /// </summary>
        /// <param name="opportunityDtos">A list of OpportunityDto objects representing the opportunities to check.</param>
        /// <param name="userId">The user ID to filter the accumulators by.</param>
        /// <returns>
        /// True if for each OpportunityDto in the list, accumulators exist with the same MatchId, OpportunityName, 
        /// and UserId, and all matching accumulators have the same GroupId. Returns false otherwise.
        Task<bool> CheckIfAccumulatorsExistAsync(List<OpportunityDto> opportunityDtos,string userId);

        Task<int> RawSqlExecute(string query);
        Task<List<List<Accumulator>>> GetAccumulatorGroupsByUserId(string userId);
    }
}
