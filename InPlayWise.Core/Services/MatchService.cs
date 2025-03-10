using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.Services
{
    public class MatchService : IMatchService
	{

		private readonly IMatchRepository _matchRepo;

		public MatchService(IMatchRepository matchRepo)
		{
			_matchRepo = matchRepo;
		}

		public async Task<Result<List<RecentMatchModel>>> GetAwayMatchesOfTeam(string teamId)
		{
			return await _matchRepo.GetAwayMatchesOfTeam(teamId);
		}

		public async Task<Result<List<RecentMatchModel>>> GetHomeMatchesOfTeam(string teamId)
		{
			return await _matchRepo.GetHomeMatchesOfTeam(teamId);
		}

		public async Task<Result<List<RecentMatchModel>>> GetRecentMatchesOfTeam(string teamId)
		{
			return await _matchRepo.GetRecentMatchesOfTeam(teamId);
		}
	}
}
