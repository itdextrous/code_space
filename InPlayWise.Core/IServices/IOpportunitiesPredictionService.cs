using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Data.DTO;
using InPlayWise.Data.DTO.OpportunitiesEntities;
using InPlayWise.Data.Entities;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
	public interface IOpportunitiesPredictionService
    {

        //Task<Result<bool>> UploadAndUpdatePredictions();
        Result<List<OpportunitiesResponseDto>> GetAllOpportunities();
        Task<Result<OpportunitiesResponseDto>> GetMatchOpportunities(string matchId);
        Task<bool> UpdateInMemoryOpportunities();

        //Task<bool> UploadDailyPredictionDataset();

        Task<bool> TrainModelAndUpdateDatabase();

        //Task<bool> DeleteDailyData();

        Task<bool> TrainModelWithAllData();

        Task<Result<Dictionary<string, int>>> GetOpportunitiesCount();

        Task<bool> SavePredictionRecord();

        Task<List<PredictionRecord>> GetPredictionRecordOfMatch(string matchId);

        Task<List<string>> GetMatchesIdWithRecord();

        Task<OpportunitiesRequestDto> GetFormattedDataForPrediction();


        //Task<bool> UploadToActiveMatchPrediction();
		//Task<bool> AddToFullPrediction(string matchId);

        Task<bool> CalculatePred(LiveMatchModel liveMatch);

        /// <summary>
        /// Retrieves the accuracy of predictions for a specified match, opportunity type, value, and event.
        /// </summary>
        /// <param name="matchId">The ID of the match for which to retrieve the accuracy.</param>
        /// <param name="type">The type of opportunity, such as Over or Under.</param>
        /// <param name="value">The threshold value for the opportunity.</param>
        /// <param name="oppEvent">The specific event associated with the opportunity.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="AccuracyDto"/> object
        /// that holds the accuracy percentage for the specified match and opportunity details.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the accuracy from the repository.</exception>
        Task<Result<AccuracyDto>> GetAccuracyByMatchIdAsync(string matchId, OpportunityType type, float value, OpportunityEvent oppEvent);

        /// <summary>
        /// Retrieves the average prediction accuracy for all entries after a given date and opportunity.
        /// </summary>
        /// <param name="date">The date after which to retrieve the prediction accuracy. The date cannot be in the future.</param>
        /// <param name="type">The type of opportunity (e.g., Over, Under).</param>
        /// <param name="value">The threshold value for the opportunity (e.g., 2.5).</param>
        /// <param name="oppEvent">The specific event related to the opportunity.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="AccuracyDto"/> object
        /// representing the average accuracy percentage for the specified date and opportunity.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the specified date is in the future.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the prediction accuracy.</exception>
        Task<Result<AccuracyDto>> GetAccuracyByDate(DateTime date, OpportunityType type, float value, OpportunityEvent oppEvent);

        /// <summary>
        /// Retrieves a list of match prediction accuracies for all entries after a specified date.
        /// </summary>
        /// <param name="date">The date after which to retrieve the prediction accuracies. The date cannot be in the future.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="MatchAccuracyDto"/> objects
        /// representing the match IDs, opportunities, and accuracies for the specified date.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the specified date is in the future.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the prediction accuracies.</exception>
        Task<Result<List<MatchAccuracyDto>>> GetMatchListAccuracy(DateTime date);

        /// <summary>
        /// Retrieves the average prediction accuracy for each type of bet opportunity.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="AccuracyPerBetDto"/> objects
        /// representing the average accuracy for each type of bet opportunity.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the prediction accuracies.</exception>
        Task<Result<List<AccuracyPerBetDto>>> GetAccuracyPerBet();

        /// <summary>
        /// Retrieves the total average prediction accuracy across all opportunities.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="AccuracyDto"/> object
        /// representing the total average accuracy percentage across all opportunities.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the prediction accuracies.</exception>
        Task<Result<AccuracyDto>> GetTotalAccuracy();

        Task<bool> RemoveEndedOpportunities();
    }
}
