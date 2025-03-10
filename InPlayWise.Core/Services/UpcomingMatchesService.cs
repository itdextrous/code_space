using InPlayWise.Common.DTO;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
    public class UpcomingMatchesService : IUpcomingMatchesService
    {

        private readonly IUpcomingMatchesRepository _umRepo;
        private readonly MatchInMemoryService _inMemory;
        private readonly ILogger<UpcomingMatchesService> _logger;
        public UpcomingMatchesService(IUpcomingMatchesRepository umRepo, MatchInMemoryService inMemory, ILogger<UpcomingMatchesService> logger)
        {
            _umRepo = umRepo;
            _inMemory = inMemory;
            _logger = logger;
        }

        public async Task<Result<bool>> DeleteOldMatches()
        {
            try
            {
                List<UpcomingMatch> matchesToDelete = new List<UpcomingMatch>();
                List<UpcomingMatch> matches = await _umRepo.GetAllUpcomingMatches();
                DateTime utcNow = DateTime.UtcNow;
                if (matches is null) return Result<bool>.InternalServerError();
                foreach(UpcomingMatch match in matches)
                {
                    if(match.time <= utcNow)
                        matchesToDelete.Add(match);
                }
                await _umRepo.DeleteMatches(matchesToDelete);
                return new Result<bool>(200, true, "Deleted the matches", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<bool>(500, false, "Internal Server Error", false);
            }
        }

        public async Task<Result<List<UpcomingMatch>>> GetAllUpcomingMatches()
        {
            try
            {
                List<UpcomingMatch> matches = _inMemory.GetUpcomingMatches();
                if (matches.Count == 0)
                {
                    await SeedUpcomingMatchesInMemory();
                    matches = _inMemory.GetUpcomingMatches();
                }

                return new Result<List<UpcomingMatch>>(200, true, "These are the matches", matches);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<List<UpcomingMatch>>(500, false, "Internal Server Error", new List<UpcomingMatch>());
            }
        }

        public async Task<Result<List<UpcomingMatch>>> SearchUpcomingMatches(string query)
        {
            List<UpcomingMatch> result = new List<UpcomingMatch>();
            try
            {
                if (query.Length < 3)
                    return new Result<List<UpcomingMatch>>(400, false, "Query length is too short", result);
                result = await SearchUpcomingMatchesInMemory(query);
                return new Result<List<UpcomingMatch>>(result is null ? 404 : 200, result is not null, result is null ? "Nothing found" : $"Found {result.Count} matches", result is not null ? result : new List<UpcomingMatch>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<List<UpcomingMatch>>(500, false, "Internal Server Error", new List<UpcomingMatch>());
            }
        }

		public async Task<bool> SeedUpcomingMatchesInMemory()
		{
            try
            {
                List<UpcomingMatch> matches = await _umRepo.GetAllUpcomingMatches();
                List<UpcomingMatch> res = new List<UpcomingMatch>();
                foreach(var match in matches)
                {
                    bool condition = match.HomeTeam?.Name is not null && match.AwayTeam?.Name is not null && match.time > DateTime.UtcNow ;
                    match.Competition.LiveMatches = null;
                    if (condition)
                        res.Add(match);
                }
                _inMemory.SetUpcomingMatches(res);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
		}

		private async Task<List<UpcomingMatch>> SearchUpcomingMatchesInMemory(string query)
        {
            try
            {
                List<UpcomingMatch> matches = _inMemory.GetUpcomingMatches();
                if(matches.Count == 0)
                {
                    await SeedUpcomingMatchesInMemory();
                }
                List<UpcomingMatch> result = new List<UpcomingMatch>();
                foreach(var match in matches)
                {
                    bool condition = match.HomeTeam.Name.ToLower().Contains(query.ToLower()) || match.AwayTeam.Name.ToLower().Contains(query.ToLower()) || match.Competition.Name.ToLower().Contains(query.ToLower());

					if (condition)
                        result.Add(match);
                    
                }
                var response = MapUpcomingMatchToUpcomingMatchSearchResponseDto(result);
                return result;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.ToString());
                return new List<UpcomingMatch>();
            }
        }




        private List<UpcomingMatchesSearchResponseDto> MapUpcomingMatchToUpcomingMatchSearchResponseDto(List<UpcomingMatch> matches)
        {
            try
            {
                List<UpcomingMatchesSearchResponseDto> matchesDto = new List<UpcomingMatchesSearchResponseDto>();
                foreach (var match in matches)
                {
                    UpcomingMatchesSearchResponseDto umr = new UpcomingMatchesSearchResponseDto()
                    {
                        Id = match.Id,
                        HomeTeamId = match.HomeTeamId,
                        AwayTeamId = match.AwayTeamId,
                        CompetitionId = match.CompetitionId,
                        HomeTeamRank = match.HomeTeamRank,
                        AwayTeamRank = match.AwayTeamRank,
                        Time = match.time,
                        HomeTeamName = match.HomeTeam.Name,
                        AwayTeamName = match.AwayTeam.Name,
                        CompetitionName = match.Competition.Name,
                        HomeTeamLogo = match.HomeTeam.Logo,
                        AwayTeamLogo = match.AwayTeam.Logo,
                        CompetitionLogo = match.Competition.Logo
                    };
                    matchesDto.Add(umr);
                }
                return matchesDto;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<UpcomingMatchesSearchResponseDto>();
            }
        }

    }
}