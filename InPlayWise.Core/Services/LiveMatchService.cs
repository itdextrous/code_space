using InPlayWise.Common.DTO;
using InPlayWise.Core.Hubs;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Mappings;
using InPlayWise.Data.DTO;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace InPlayWise.Core.Services
{
    public class LiveMatchService : ILiveMatchService
	{
		private readonly IBasicDataServices _basicDataService;
		private readonly IBasicInfoServices _basicInfo;
		private readonly ILiveMatchRepository _liveRepo;
		private readonly ILogger<LiveMatchService> _logger;
		private readonly MatchInMemoryService _inMemoryMatch;
		private readonly ILeagueStatsService _leagueStats;
		private readonly IOpportunitiesPredictionService _predService;
		private readonly IInsightService _insights;
		private readonly IHubContext<MatchesHub> _matchesHub;

		public LiveMatchService(IBasicDataServices basicDataServices, ILiveMatchRepository liveRepo, ILogger<LiveMatchService> logger, MatchInMemoryService inMemoryMatch, ILeagueStatsService leagueStats, IBasicInfoServices basicInfo, IOpportunitiesPredictionService predService, IInsightService insights, IHubContext<MatchesHub> matchesHub)
		{
			_basicDataService = basicDataServices;
			_liveRepo = liveRepo;
			_logger = logger;
			_leagueStats = leagueStats;
			_inMemoryMatch = inMemoryMatch;
			_basicInfo = basicInfo;			_predService = predService;
			_insights = insights;
			_matchesHub = matchesHub;
		}

		

		public Result<List<LiveMatchDto>> GetAllLiveMatches()
		{
			try
			{
				return new Result<List<LiveMatchDto>>(200, true, "All matches", _inMemoryMatch.GetAllLiveMatches());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<LiveMatchDto>>(500, false, "Internal server error", new List<LiveMatchDto>());
			}
		}

		public async Task<LiveMatchModel> GetLiveMatch(string matchId)
		{
			try
			{
				var match = await _liveRepo.GetLiveMatchById(matchId);
				return match;
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return new LiveMatchModel();
			}
		}

		public async Task<Result<LiveMatchModel>> LiveMatchInfo(string matchId)
		{
			try
			{
				var match = await _liveRepo.GetLiveMatchById(matchId);
				return new Result<LiveMatchModel>
				{
					IsSuccess = match is not null,
					Items = match,
					Message = match is null ? "No match was found" : "",
					StatusCode = match is null ? 400 : 200
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<LiveMatchModel>
				{
					IsSuccess = false,
					Message = "Server side error occurred"
				};
			}
		}

		public async Task<Result<List<LiveMatchModel>>> GetCompleteLiveMatches()
		{
			try
			{
				var match = await _liveRepo.GetAllLiveMatches();
				return new Result<List<LiveMatchModel>>
				{
					IsSuccess = match is not null,
					Items = match,
					Message = match is null ? "No matches were found" : "",
					StatusCode = match is null ? 400 : 200
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<LiveMatchModel>>
				{
					IsSuccess = false,
					Message = "Server side error occurred"
				};
			}
		}

		public async Task<Result<List<LiveMatchBasicDto>>> AllLiveMatchesBasicInfo()
		{
			try
			{
				List<LiveMatchModel> allMatches = await _liveRepo.GetAllLiveMatches();
				List<LiveMatchBasicDto> matches = new List<LiveMatchBasicDto>();

				foreach(var match in allMatches)
				{
					LiveMatchBasicDto basicMatch = MappingService.MapLiveModelToLiveBasicModel(match);
					matches.Add(basicMatch);
				}

				return new Result<List<LiveMatchBasicDto>>()
				{
					IsSuccess = true,
					Items = matches,
					Message = $"{matches.Count} matches are going on",
					StatusCode = 200
				};

			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<LiveMatchBasicDto>>
				{
					IsSuccess = false,
					Message = "Internal server error",
					StatusCode = 500
				};  
			}
		}


		public async Task<Result<LiveMatchBasicDto>> GetLiveMatchBasicInfo(string matchId)
		{
			try
			{
				LiveMatchModel fullMatch = await _liveRepo.GetLiveMatchById(matchId);
				LiveMatchBasicDto match = MappingService.MapLiveModelToLiveBasicModel(fullMatch);
				return new Result<LiveMatchBasicDto>()
				{
					IsSuccess = true,
					Items = match,
					Message = "match found",
					StatusCode = 200
				};

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<LiveMatchBasicDto>
				{
					IsSuccess = false,
					Message = "Internal server error",
					StatusCode = 500
				};
			}
		}



		public async Task<Result<SearchGamesResponseDto>> SearchMatches(string query)
		{
			SearchGamesResponseDto result = new SearchGamesResponseDto();
			try
			{
				if(query.Length < 3)
					return new Result<SearchGamesResponseDto>(400, false, "Query length is too short", result);
				List<LiveMatchDto> liveMatches = SearchLiveMatches(query);
				List<UpcomingMatch> upcomingMatches = SearchUpcomingMatchesInMemory(query);
				List<RecentMatchModel> recentMatches = await SearchRecentMatches(query);
				result.LiveMatches = liveMatches;
				result.UpcomingMatches = upcomingMatches;
				result.RecentMatches = recentMatches;
				return new Result<SearchGamesResponseDto>(200, true, $"Total matches = {liveMatches.Count + upcomingMatches.Count + recentMatches.Count}", result);
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<SearchGamesResponseDto>(500, false, "Internal Server error", result);
			}
		}


		private List<LiveMatchDto> SearchLiveMatches(string query)
		{
			try
			{
				List<LiveMatchDto> matches = _inMemoryMatch.GetAllLiveMatches();
				List<LiveMatchDto> result = new List<LiveMatchDto>();
				foreach(var match in matches)
				{
					if(match.HomeTeamName.ToLower().Contains(query.ToLower()) || match.AwayTeamName.ToLower().Contains(query.ToLower()) || match.CompetitionName.ToLower().Contains(query.ToLower()))
					{
						result.Add(match);
					}
				}
				return result;
			}catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new List<LiveMatchDto>();
			}
		}



		private async Task<List<RecentMatchModel>> SearchRecentMatches(string query)
		{
			try
			{
				// below is old implementation
				//List<string> teamIds = await _liveRepo.SearchTeamName(query);
				//List<RecentMatchModel> matches = await _liveRepo.GetMatchesById(teamIds);
				//return matches;

				// below is new implementation
				int length = _inMemoryMatch.GetTeams().Count();
				List<Team> teams = _inMemoryMatch.GetTeams().Where(tm => tm.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
					.ToList();
				List<RecentMatchModel> result = new List<RecentMatchModel>();
				Dictionary<string, List<RecentMatchModel>> pastMatches = _inMemoryMatch.GetPastMatchesHash();
				foreach(Team team in teams)
				{
					List<RecentMatchModel> matches = pastMatches.ContainsKey(team.Id) ? pastMatches[team.Id] : new List<RecentMatchModel>();
					foreach(var match in matches)
					{
						if (match.HomeTeamId.Equals(team.Id))
						{
							match.HomeTeam = team;
							match.AwayTeam = _inMemoryMatch.GetTeams().FirstOrDefault(tm => tm.Id.Equals(match.AwayTeamId));
						}
						else
						{
							match.AwayTeam = team;
							match.HomeTeam = _inMemoryMatch.GetTeams().FirstOrDefault(tm => tm.Id.Equals(match.AwayTeamId));
						}
						if(match.HomeTeam is not null)
						{
                            match.HomeTeam.HomeRecentMatches = null;
                            match.HomeTeam.AwayRecentMatches = null;
                        }
						if(match.AwayTeam is not null)
						{
                            match.AwayTeam.HomeRecentMatches = null;
                            match.AwayTeam.AwayRecentMatches = null;
                        }
						//match.HomeTeam.HomeRecentMatches = null;
						//match.HomeTeam.AwayRecentMatches = null;
						//match.AwayTeam.HomeRecentMatches = null;
						//match.AwayTeam.AwayRecentMatches = null;
						match.Competition = _inMemoryMatch.GetCompetitions().FirstOrDefault(comp => comp.Id.Equals(match.CompetitionId));
						if(match.Competition is not null)
						{
							match.Competition.RecentMatches = null;
						}
						result.Add(match);
					}
					result.AddRange(matches);
				}
				return result;
			}catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new List<RecentMatchModel>();
			}
		}

		private List<UpcomingMatch> SearchUpcomingMatchesInMemory(string query)
		{
			try
			{
				List<UpcomingMatch> matches = _inMemoryMatch.GetUpcomingMatches();
				List<UpcomingMatch> result = new List<UpcomingMatch>();
				foreach (var match in matches)
				{
					bool condition = match.HomeTeam?.Name is not null && match.AwayTeam?.Name is not null && match.Competition?.Name is not null && (match.HomeTeam.Name.ToLower().Contains(query.ToLower()) || match.AwayTeam.Name.ToLower().Contains(query.ToLower()) || match.Competition.Name.ToLower().Contains(query.ToLower()));

					if (condition)
						result.Add(match);

				}
				//return MapUpcomingMatchToUpcomingMatchSearchResponseDto(result);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new List<UpcomingMatch>();
			}
		}

		public async Task<Result<bool>> BroadcastMatches()
		{
			try
			{
				List<LiveMatchDto> currentMatches = _inMemoryMatch.GetAllLiveMatches();
				List<LiveMatchDto> previousMatches = _inMemoryMatch.GetPreviousLiveMatches();
				Dictionary<string, Dictionary<string, object>> res = new Dictionary<string, Dictionary<string, object>>();

				foreach (var curMatch in currentMatches)
				{
					Type type = curMatch.GetType();
					PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

					if (!previousMatches.Select(match => match.MatchId).Contains(curMatch.MatchId))
					{
						Dictionary<string, object> matchNew = new Dictionary<string, object>();
						foreach (PropertyInfo property in properties)
							matchNew[property.Name] = property.GetValue(curMatch);
						res[curMatch.MatchId] = matchNew;
						continue;
					}
					LiveMatchDto prevMatch = previousMatches.SingleOrDefault(match => match.MatchId.Equals(curMatch.MatchId));
					if (prevMatch is null) continue;
					Dictionary<string, object> resMatch = new Dictionary<string, object>();
					foreach (PropertyInfo property in properties)
					{
						if (property.PropertyType == typeof(InsightsDto))
							continue;
						object curValue = property.GetValue(curMatch);
						object prevValue = property.GetValue(prevMatch);
						if(!object.Equals(curValue, prevValue))
							resMatch[property.Name] = curValue;
					}
					if(resMatch.Count > 0)
						res[curMatch.MatchId] = resMatch;
				}
				await _matchesHub.Clients.All.SendAsync("updateMatch", res );
				return Result<bool>.Success();
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return Result<bool>.InternalServerError();
			}
		}

        public async Task<Result<List<LiveMatchFilterDto>>> AllLiveMatchesFilter()
        {
            try
            {
				return Result<List<LiveMatchFilterDto>>.Success("filters",_inMemoryMatch.GetLiveFilters());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<LiveMatchFilterDto>>.InternalServerError();
            }
        }
    }
}
