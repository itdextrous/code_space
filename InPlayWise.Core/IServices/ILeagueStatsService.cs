using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Data.Entities;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface ILeagueStatsService
    {

        Task<Result<List<LeagueStats>>> GetTenLeagueStats();
        Task<Result<LeagueStats>> GetStatsOfLeague(string leagueId);

        Task<Result<List<LeagueStats>>> GetStatsOfLeagueMultiple(List<string> leagueId);
        Task<bool> AddOrUpdateLeagueStats(LiveMatchModel match);

        Task<bool> UpdateStatsForLeague(string CompetitionId);

        Task<LeagueStats> GetStatsOfLeagueFromApi(string leageId, List<ApiSeason> allSeasons = null);

        Task<Result<bool>> SetFavouriteLeagueStats(List<string> leagueIds);
        Task<Result<List<string>>> GetFavouriteLeagueStatIds();
        Task<Result<List<LeagueStats>>> GetFavouriteLeagueStats();
        Task<Result<List<LeagueStats>>> GetMultipleLeagueStats(List<string> leageIds);

    }
}
