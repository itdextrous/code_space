using InPlayWise.Data.Entities.SportsEntities;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWise.Core.IServices
{
    public interface ITeamsService
    {
        Task<Result<List<Team>>> GetFiftyTeams();
        Task<Result<List<Team>>> SearchTeamsByName(string teamName);
        Task<Result<List<Team>>> SearchTeamsByCountry(string countryName);
        Task<Result<List<Team>>> SearchTeamsByCompetition(string competitionName);
        Task<Result<Team>> GetTeamById(string teamId);

    }
}
