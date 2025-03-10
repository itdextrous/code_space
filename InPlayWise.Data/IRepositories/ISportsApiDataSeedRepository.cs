using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface ISportsApiDataSeedRepository
	{
		Task<bool> AddCategories(List<Category> categories);
		Task<bool> AddCountries(List<Country> countries);
		Task<bool> AddCompetition(List<Competition> competitions);
		Task<bool> AddTeams(List<Team> teams);
		Task<bool> TeamExists(string teamId);
		Task<bool> AddTeam(Team team);
		Task<bool> AddOrUpdateTeam(Team team);
		Task<bool> AddOrUpdateCompetition(Competition comp);
		Task<bool> AddSeasons(List<Season> seasons);
		Task<bool> AddUpcomingMatches(List<UpcomingMatch> matches);
		Task<List<string>> GetAllTeamsId();
		Task<List<string>> GetAllCompetitionId();
		Task<bool> SaveTeamsList(List<Team> teams);
		Task<bool> SaveCompetitionList(List<Competition> competitions);
		Task<List<UpcomingMatch>> GetAllUpcomingMatches();
		Task<List<string>> GetIdOfExistingUpcomingMatches();
		Task<bool> DeleteAllUpcomingMatches();
		Task<List<Competition>> GetAllCompetitions();
		Task<bool> UpdateTeamCount(List<Competition> comps);
		Task<List<string>> GetTeamsWithIncompleteInfo();
		Task<List<string>> GetAllCategoriesId();
		Task<List<string>> GetAllContriesId();
		Task<List<string>> GetCompetitionsWithIncompleteInfo();
		Task<bool> UpdateMultipleTeams(List<Team> teams);
		Task<bool> UpdateMultipleCompeitions(List<Competition> competitions);
		Task<List<Team>> GetAllTeams();
		Task<List<Competition>> GetAllLeaguesWithoutTeamCount();
	}
}
