using AutoMapper;
using InPlayWise.Common.DTO;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
    public class RecentMatchesService : IRecentMatchService
    {

        private readonly IRecentMatchRepository _recentMatchRepo;
        private readonly ILogger<RecentMatchesService> _logger;
        private readonly MatchInMemoryService _inMemory;
        private readonly IBasicDataServices _basicDataServices;
        private readonly IMapper _mapper;



        public RecentMatchesService(IRecentMatchRepository matchRepo, ILogger<RecentMatchesService> logger, MatchInMemoryService inMemory, IBasicDataServices basicDataServices, IMapper mapper)
        {
            _recentMatchRepo = matchRepo;
            _logger = logger;
            _inMemory = inMemory;
            _basicDataServices = basicDataServices;
            _mapper = mapper;
        }


		public async Task<Result<RecentMatchDto>> MatchById(string matchId)
        {
            try
            {
                //RecentMatchModel match = _inMemory.GetRecentMatches().Where(m => m.MatchId.Equals(matchId)).SingleOrDefault();
                RecentMatchModel match = await _recentMatchRepo.GetMatchById(matchId);
                RecentMatchDto result = _mapper.Map<RecentMatchDto>(match);
                return match is null ? Result<RecentMatchDto>.NotFound("Match not found") : Result<RecentMatchDto>.Success("Match found", result);

            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Result<RecentMatchDto>.InternalServerError();
            }
        }

        public async Task<Result<List<RecentMatchDto>>> Last50Matches()
        {
            try
            {
                //List<RecentMatchModel> matches = _inMemory.GetRecentMatches().Take(50).ToList();
                List<RecentMatchModel> matches = await _recentMatchRepo.GetLast50Matches();
                List<RecentMatchDto> result = matches.Select(match => _mapper.Map<RecentMatchDto>(match)).ToList();
                return Result<List<RecentMatchDto>>.Success("", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<List<RecentMatchDto>>.InternalServerError();
            }
        }


        public async Task<Result<List<RecentMatchDto>>> MatchesByCompetition(string competitionId)
        {
            try
            {
                //List<RecentMatchModel> matches = _inMemory.GetRecentMatches().Where(m => m.CompetitionId.Equals(competitionId)).ToList();
                List<RecentMatchModel> matches = await _recentMatchRepo.GetMatchesByCompetition(competitionId);
                List<RecentMatchDto> result = matches.Select(match => _mapper.Map<RecentMatchDto>(match)).ToList();
                return Result<List<RecentMatchDto>>.Success("", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<List<RecentMatchDto>>.InternalServerError();
            }
        }

        public async Task<Result<List<RecentMatchDto>>> MatchesOfTeamById(string teamId)
        {
            try
            {
                //List<RecentMatchModel> matches = _inMemory.GetRecentMatches().Where(m => m.HomeTeamId.Equals(teamId) || m.AwayTeamId.Equals(teamId)).ToList();
                List<RecentMatchModel> matches = _inMemory.GetPastMatchesHash()[teamId] ?? new List<RecentMatchModel>();
                List<RecentMatchDto> result = matches.Select(match => _mapper.Map<RecentMatchDto>(match)).ToList();
                return Result<List<RecentMatchDto>>.Success(item: result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<List<RecentMatchDto>>.InternalServerError();
            }
        }

        public async Task<Result<List<RecentMatchDto>>> GetLastThreeMatchesOfTeam(string teamId)
        {
            try
            {

                //List<RecentMatchModel> matches = _inMemory.GetRecentMatches().Where(m => m.HomeTeamId.Equals(teamId) || m.AwayTeamId.Equals(teamId)).Where(rm => rm.MatchStartTimeOfficial > DateTime.UtcNow.AddDays(-31.5)).Take(3).ToList();
                List<RecentMatchModel> matches = _inMemory.GetPastMatchesHash()[teamId].Take(3).ToList() ?? new List<RecentMatchModel>() ;
                List<RecentMatchDto> result = matches.Select(match => _mapper.Map<RecentMatchDto>(match)).ToList();
                return Result<List<RecentMatchDto>>.Success("", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<List<RecentMatchDto>>.InternalServerError();
            }
        }

        public async Task<Result<List<RecentMatchDto>>> GetLastNMatchesOfTeam(string teamId, int n)
        {
            try
            {
                //List<RecentMatchModel> matches = _inMemory.GetRecentMatches().Where(m => m.HomeTeamId.Equals(teamId) || m.AwayTeamId.Equals(teamId)).Take(n).ToList();
                List<RecentMatchModel> matches = _inMemory.GetPastMatchesHash()[teamId].Take(n).ToList() ?? new List<RecentMatchModel>();
                List<RecentMatchDto> result = matches.Select(match => _mapper.Map<RecentMatchDto>(match)).ToList();
                return Result<List<RecentMatchDto>>.Success("", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<List<RecentMatchDto>>.InternalServerError();
            }
        }



		public async Task<Result<RecentMatchDto>> GetMatchFromDb(string matchId)
		{
			try
			{
				RecentMatchModel match = await _recentMatchRepo.GetMatchById(matchId);
                RecentMatchDto result = _mapper.Map<RecentMatchDto>(match);
				return Result<RecentMatchDto>.Success("", result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<RecentMatchDto>.InternalServerError();
			}
		}

		public async Task<Result<bool>> GetHistoricalMatchesOnLocal()
		{
            try
            {
                List<RecentMatchModel> matches = await _recentMatchRepo.GetAllMatches();
                bool saved = await _recentMatchRepo.LocalDbUploadMatches(matches);
                if (saved)
                    return Result<bool>.Success("", true);
                return Result<bool>.InternalServerError("failed");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError();
            }
		}




        public async Task<Result<object>> TestPoint()
		{
            try
            {
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
		}

		public async Task<List<RecentMatchDto>> GetMatchesOfTeamFromDbByTeamid(string teamId)
		{
            try
            {
                List<RecentMatchModel> matches = await _recentMatchRepo.GetMatchesOfTeamById(teamId);
                List<RecentMatchDto> result = matches.Select(match => _mapper.Map<RecentMatchDto>(match)).ToList();
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
		}
	}
}
