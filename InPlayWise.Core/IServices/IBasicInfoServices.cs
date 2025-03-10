using InPlayWise.Common.DTO.FootballResponseModels.BasicInfoResponseModels;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IBasicInfoServices
    {
        Task<List<ApiCategory>> Category();
        Task<List<ApiCountry>> Country();
        Task<List<ApiCompetition>> Competition(string competitionId = "");
        Task<List<ApiTeam>> Team(string teamId = "");
        //Task<Result<VenueResponseModel>> Venue(string uuid = "");
        Task<List<ApiSeason>> Season(string seasonId = "");
        Task<List<ApiStage>> Stage(string uuid = "");
        //Task<Result<DataUpdateResponseModel>> DataUpdate();

    }
}
