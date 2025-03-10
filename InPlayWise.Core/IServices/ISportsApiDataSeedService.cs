using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWise.Core.IServices
{
	public interface ISportsApiDataSeedService
	{
		Task<bool> SeedCategory();
		Task<bool> SeedCountry() ;
		Task<bool> SeedCompetitions() ;
		Task<bool> SeedTeams();
		//Task<Result<bool>> SeedUpdatedTeams();
		//Task<Result<bool>> SeedAllTeamsUpdate();
		Task<bool> SeedSeason() ;
		Task<bool> SeedUpcomingMatches();
		//Task<Result<bool>> SeedUpdatedCompetitions();
		Task<bool> SeedTeamCount();

	}
}
