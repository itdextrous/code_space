using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
    public interface ITeamsRepository
    {

        Task<List<Team>> GetFiftyTeams();
        Task<List<Team>> GetTeamsByName(string teamName);
        Task<List<Team>> GetTeamsByCompetitionName(string competitionName);
        Task<List<Team>> GetTeamsByCountryName(string competitionName);
        Task<Team> GetTeamById(string teamId);
        Task<List<Team>> GetAllTeams();
        Task<List<Competition>> GetAllCompetions();

    }
}
