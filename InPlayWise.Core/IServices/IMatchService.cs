using InPlayWise.Data.Entities.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IMatchService
	{
		public Task<Result<List<RecentMatchModel>>> GetRecentMatchesOfTeam(string teamId);
		public Task<Result<List<RecentMatchModel>>> GetHomeMatchesOfTeam(string teamId);
		public Task<Result<List<RecentMatchModel>>> GetAwayMatchesOfTeam(string teamId);


	}
}
