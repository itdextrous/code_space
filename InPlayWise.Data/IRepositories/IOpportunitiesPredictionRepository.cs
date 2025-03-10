using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.Prediction;
using InPlayWise.Data.Entities.PredictionEntities;
using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IOpportunitiesPredictionRepository
    {
        Task<List<LiveMatchModel>> GetAllLiveMatches();

        Task<bool> DeleteAndRefillOpportunities(List<Opportunity> opportunities);

        Task<List<Opportunity>> GetAllOpportunities();

        Task<List<Opportunity>> GetOpportunitiesByMatchId(string matchId);




        Task<PlanFeatures> GetPlanFeatures(string userId);
        Task<List<PredictionCounter>> GetUserHitsOnMatches(string userId);
        Task<bool> AddCounter(PredictionCounter counter);
        Task<bool> UpdateCounter(PredictionCounter counter);





        //Task<bool> AddToDailyPredictionDataset(List<DailyPredictionDataset> data);
        //Task<bool> AddToAllPredictionDataset(List<AllPredictionDataset> data);
        //Task<bool> DeleteFromDailyPredictionDataset();
        //Task<List<DailyPredictionDataset>> GetDailyPredictionDataset();
        //Task<List<AllPredictionDataset>> GetAllPredictionDatasets(int n);
        Task<bool> SavePredictionRecords(List<PredictionRecord> preds);
        Task<List<PredictionRecord>> GetRecordByMatchId(string matchId);
        Task<List<string>> GetMatchesIdWithRecords();




        //Task<bool> UploadToActiveMatchesPrediction(List<PredictionActiveMatchesData> dataset);
        //Task<List<PredictionActiveMatchesData>> GetPredictionDatasetForActiveMatch(string matchId);
        //Task<bool> UploadToFullPredictionData(List<PredictionFullData> dataset);

        /// <summary>
        /// Retrieves the accuracy of predictions for a specified match and opportunity.
        /// </summary>
        /// <param name="matchId">The ID of the match for which to retrieve the accuracy.</param>
        /// <param name="opportunity">The opportunity name for which to retrieve the accuracy.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the accuracy value for the specified match and opportunity.
        /// If no matching record is found, the default value of <see cref="float"/> is returned.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while querying the database.</exception>
        Task<float> GetAccuracyByMatchIdAsync(string matchId, string opportunity);

        /// <summary>
        /// Retrieves the average accuracy of predictions for a specified opportunity after a given date.
        /// </summary>
        /// <param name="date">The date after which to retrieve the prediction accuracies.</param>
        /// <param name="opportunity">The opportunity name for which to calculate the average accuracy.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the average accuracy percentage
        /// for the specified opportunity and date. If no records are found, the default value of <see cref="float"/> is returned.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while querying the database.</exception>
        Task<float> GetAccuracyByDate(DateTime date, string opportunity);

        /// <summary>
        /// Retrieves all prediction accuracy records for matches occurring after a specified date.
        /// </summary>
        /// <param name="date">The date after which to retrieve the prediction accuracy records.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="PredictionAccuracy"/>
        /// objects representing all records in the PredictionAccuracies table where the EndTime is greater than the specified date.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while querying the database.</exception>
        Task<List<PredictionAccuracy>> GetMatchListAccuracy(DateTime date);

        /// <summary>
        /// Retrieves all prediction accuracy records from the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="PredictionAccuracy"/>
        /// objects representing all the records in the PredictionAccuracies table.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while querying the database.</exception>
        Task<List<PredictionAccuracy>> GetAllAccuracies();

        /// <summary>
        /// Saves a list of prediction accuracy records to the database.
        /// </summary>
        /// <param name="predictionAccuracies">A list of <see cref="PredictionAccuracy"/> objects to be saved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean value indicating whether
        /// the save operation was successful. Returns <c>true</c> if the records were successfully added and saved; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while saving the records to the database.</exception>
        Task<bool> SavePredictionAccuracies(List<PredictionAccuracy> predictionAccuracies);

        /// <summary>
        /// Deletes all prediction records associated with a specified match ID.
        /// </summary>
        /// <param name="matchId">The ID of the match for which the prediction records are to be deleted.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean value indicating whether
        /// the delete operation was successful. Returns <c>true</c> if the records were successfully removed; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while deleting the records from the database.</exception>
        Task<bool> DeletePredictionRecord(string matchId);
    }
}
