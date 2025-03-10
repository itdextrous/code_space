using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface IHistoricalStatsRepository
    {
        Task<List<RecentMatchModel>> GetLastMatchesOfTeam(string teamId);
        Task<LiveMatchModel> GetLiveMatchById(string matchId);

        Task<UpcomingMatch> GetUpcomingMatchById(string matchId);
        Task<List<RecentMatchModel>> GetPastMatchesOfTeam(string teamId);
    }
}
