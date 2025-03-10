using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.Services
{
    public class TeamsService : ITeamsService
    {

        private readonly ITeamsRepository _teamsRepo;

        public TeamsService(ITeamsRepository teamRepo)
        {
            _teamsRepo = teamRepo;
        }

        public async Task<Result<List<Team>>> GetFiftyTeams()
        {
            try
            {
                List<Team> tms = await _teamsRepo.GetFiftyTeams();
                return new Result<List<Team>>
                {
                    Items = tms,
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "These are the resulting teams"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<List<Team>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };
            }
        }

        public async Task<Result<Team>> GetTeamById(string teamId)
        {
            try
            {
                Team team = await _teamsRepo.GetTeamById(teamId);
                return new Result<Team>
                {
                    Items = team,
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "These are the resulting teams"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<Team>
                {
                    Items = null,
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };
            }
        }

        public async Task<Result<List<Team>>> SearchTeamsByCompetition(string competitionName)
        {
            try
            {
                if(competitionName.Length < 3) {
                    return new Result<List<Team>>
                    {
                        IsSuccess = false,
                        StatusCode = 400
                    };
                }
                var teams =  await _teamsRepo.GetTeamsByCompetitionName(competitionName);
                return new Result<List<Team>>
                {
                    Items = teams,
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "These are the resulting teams"
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<List<Team>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };
            }
        }

        public async Task<Result<List<Team>>> SearchTeamsByCountry(string countryName)
        {
            try
            {

                if (countryName.Length < 3)
                {
                    return new Result<List<Team>>
                    {
                        IsSuccess = false,
                        StatusCode = 400
                    };
                }
                var teams = await _teamsRepo.GetTeamsByCountryName(countryName);
                return new Result<List<Team>>
                {
                    Items = teams,
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "These are the resulting teams"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<List<Team>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };
            }
        }

        public async Task<Result<List<Team>>> SearchTeamsByName(string teamName)
        {
            try
            {
                if (teamName.Length < 3)
                {
                    return new Result<List<Team>>
                    {
                        IsSuccess = false,
                        StatusCode = 400
                    };
                }
                List<Team> teams = await _teamsRepo.GetTeamsByName(teamName);
                return new Result<List<Team>>
                {
                    Items = teams,
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "These are the resulting teams"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<List<Team>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };
            }
        }

        
    }
}
