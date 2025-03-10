using InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Data.SportsEntities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InPlayWise.Core.IServices
{
    public interface IBasicDataServices
    {

        Task<List<ApiRecentMatch>> MatchRecent(string matchId = "", long timeStamp = 0);
        Task<List<ApiRecentMatch>> ScheduleAndResultsDateQuery(long tsp = 0);
        Task<List<RecentMatchResponseDto>> ScheduleAndResultsSeasonQuery(string seasonId = "");
        Task<List<LiveMatchModel>> RealTimeData();
        Task<JsonResult> MatchTimelines(string uuid = "");
        Task<JsonResult> SingleMatchLineUp(string uuid = "");
        Task<JsonResult> MatchPlayerStatistics(string uuid = "");
        Task<JsonResult> MatchTeamStatistics(string uuid = "");
        Task<JsonResult> H2H(string uuid = "");
        Task<JsonResult> SeasonStandings(string uuid = "");
        Task<DataList> StatisticalData(string uuid = "");
        Task<JsonResult> PlayerStatistics(string uuid = "");
        Task<JsonResult> TeamStatistics(string uuid = "");
        Task<JsonResult> HistoricalCompensation(string uuid = "");
        Task<JsonResult> RealTimeStandings(string uuid = "");
        Task<JsonResult> Delete(string uuid = "");
        Task<JsonResult> Competition(string uuid = "");
        Task<JsonResult> TeamLineup(string uuid = "" );
        Task<JsonElement> RealTimeDataTest();
        Task<object?> GetIpAddress(string url);

        //Task<int> FetchTeamStandings(string seasonId, string teamId);

    }
}
