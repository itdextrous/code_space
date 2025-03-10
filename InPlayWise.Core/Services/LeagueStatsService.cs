using InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
    public class LeagueStatsService : ILeagueStatsService
    {
        private readonly ILeagueStatsRepository _lsRepo;
        private readonly ILogger<LeagueStatsService> _logger;
        private readonly IHttpContextService _httpContext;
        private readonly IBasicDataServices _basicData;
        private readonly IBasicInfoServices _basicInfo;
        private readonly MatchInMemoryService _inMemory;
        private readonly UserManager<ApplicationUser> _user;
        public LeagueStatsService(ILeagueStatsRepository lsRepo, ILogger<LeagueStatsService> logger, IHttpContextService context, IBasicDataServices basicData, IBasicInfoServices basicInfo, MatchInMemoryService memory, UserManager<ApplicationUser> userManager)
        {
            _lsRepo = lsRepo;
            _logger = logger;
            _httpContext = context;
            _basicData = basicData;
            _basicInfo = basicInfo;
            _inMemory = memory;
            _user = userManager;
        }

        public async Task<Result<LeagueStats>> GetStatsOfLeague(string leagueId)
        {
            try
            {

                LeagueStats stat = await _lsRepo.GetLeagueStatsByLeagueId(leagueId);
                bool suc = stat is not null;
                return new Result<LeagueStats>(suc ? 200 : 404, suc, suc ? "Found stats" : "Not found", stat);
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<LeagueStats>(500, false, "Internal Server Erro", null);
            }
        }

        public async Task<Result<List<LeagueStats>>> GetTenLeagueStats()
        {
            try
            {
                List<string> desiredId = new List<string>()
                {
                    "jednm9whz0ryox8",
                    "vl7oqdehlyr510j",
                    "gy0or5jhg6qwzv3",
                    "4zp5rzghp5q82w1",
                    "yl5ergphnzr8k0o",
                    "kjw2r09hv5rz84o",
                    "p3glrw7hzkvqdyj",
                    "9vjxm8ghx2r6odg",
                    "vl7oqdeheyr510j",
                    "kn54qllhg2qvy9d",
                    "p3glrw7hevqdyjv"
                };


                List<LeagueStats> saved = _inMemory.GetLeagueStatsTopTen();
                bool condition = saved is null || saved.Count == 0 || saved[0].UpdatedTime < DateTime.UtcNow.AddMonths(-1);
                if (!condition)
                    return Result<List<LeagueStats>>.Success("Cached League stats", saved);

                List<LeagueStats> result = new List<LeagueStats>();
                List<ApiSeason> allSeasons = await _basicInfo.Season(); // Added here to prevent fetching all seasons info for each league
                foreach (string id in desiredId)
                {
                    LeagueStats stat = null;
                    List<ApiSeason> seasons = allSeasons.Where(s => s.CompetitionId == id)
                        .OrderByDescending(s => s.StartTime).ToList();
                    ApiSeason currentSeason = seasons.FirstOrDefault(cs => cs.IsCurrent == 1);
                    if (currentSeason is null) continue;
                    if (DateTime.UtcNow < DateTimeOffset.FromUnixTimeSeconds(currentSeason.StartTime).UtcDateTime)
                    {
                        stat = await GetLeagueStatFromDb(seasons[1].Id);
                        stat.Competition = await _lsRepo.GetCompetition(id);
                        stat.Competition.RecentMatches = null;
                    }
                    else 
                    {
                        stat = await GetLeagueStatFromSportsApi(id);
                    }
                    if(stat is not null)
                    {
                        stat.UpdatedTime = DateTime.UtcNow;
                        result.Add(stat);
                    }
                }
                _inMemory.SetLeagueStatsTopTen(result);
                return Result<List<LeagueStats>>.Success("League stats", result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<List<LeagueStats>>(500, false, "Internal Server Erro", new List<LeagueStats>());
            }
        }


        public async Task<Result<List<LeagueStats>>> GetMultipleLeagueStats(List<string> leagueIds)
        {
            try
            {
                List<LeagueStats> result = new List<LeagueStats>();
                List<ApiSeason> allSeasons = new List<ApiSeason>();
                foreach (string id in leagueIds)
                {
                    LeagueStats stat = null;
                    if (string.IsNullOrEmpty(id)) continue;
                    LeagueStats savedStat = _inMemory.GetLeagueStatsAll().FirstOrDefault(ls => ls.CompetitionId == id);
                    if (savedStat is not null && savedStat.UpdatedTime > DateTime.UtcNow.AddHours(-1))
                    {
                        result.Add(savedStat);
                        continue;
                    }
                    if (allSeasons is null || allSeasons.Count == 0) { allSeasons = await _basicInfo.Season(); }
                    
                    List<ApiSeason> seasons = allSeasons.Where(s => s.CompetitionId == id)
                        .OrderByDescending(s => s.StartTime).ToList();
                    ApiSeason currentSeason = seasons.FirstOrDefault(cs => cs.IsCurrent == 1);
                    if (currentSeason is null) continue;
                    if (DateTime.UtcNow < DateTimeOffset.FromUnixTimeSeconds(currentSeason.StartTime).UtcDateTime)
                    {
                        stat = await GetLeagueStatFromDb(seasons[1].Id);
                    }
                    else
                    {
                        stat = await GetLeagueStatFromSportsApi(id);
                    }
                    if (stat is not null)
                    {
                        stat.UpdatedTime = DateTime.UtcNow;
                        result.Add(stat);
                    }               
                }
                List<LeagueStats> allStatsInMemory = _inMemory.GetLeagueStatsAll();
                allStatsInMemory.RemoveAll(stat => leagueIds.Contains(stat.CompetitionId));
                allStatsInMemory.AddRange(result);
                _inMemory.SetLeagueStatsAll(allStatsInMemory);
                return Result<List<LeagueStats>>.Success("League Stats", result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<LeagueStats>>.InternalServerError() ;
            }
        }

        public async Task<bool> AddOrUpdateLeagueStats(LiveMatchModel match)
        {
            try
            {
                string seasonId = match.SeasonId;
                if (string.IsNullOrEmpty(seasonId)) return false;
                LeagueStats exStats = await _lsRepo.GetLeagueStatsByLeagueId(match.CompetitionId);
                if ((exStats is not null) &&  (!exStats.SeasonId.Equals(match.SeasonId)))
                {
                    bool deleted = await _lsRepo.DeleteLeagueStats(match.CompetitionId);
                    if (!deleted) return false;
                    exStats = null;
                }
                LeagueStats updatedStats = GetLeagueStatsFromLiveMatch(match, exStats);
                if(exStats is null)
                {
                    bool added = await _lsRepo.AddLeagueStats(updatedStats);
                    return added;
                }
                else
                {
                    bool updated = await _lsRepo.UpdateLeagueStats(updatedStats);
                    return updated;
                }

            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }


        private LeagueStats GetLeagueStatsFromLiveMatch(LiveMatchModel match, LeagueStats oldStats)
        {
            try
            {
                int totalGoals = match.HomeGoals + match.AwayGoals;
                int totalCards = match.HomeYellow + match.HomeRed + match.AwayYellow + match.AwayRed;
                int totalCorners = match.HomeCorners + match.AwayCorners;
                bool btts = match.HomeGoals >= 1 && match.AwayGoals >= 1;
                if(oldStats is null)
                {
                    return new LeagueStats()
                    {
                        CompetitionId = match.CompetitionId,
                        SeasonId = match.SeasonId,
                        OverOnePointFive = totalGoals >= 2 ? 1 : 0,
                        OverTwoPointFive = totalGoals >= 3 ? 1 : 0,
                        BTTS = btts ? 1 : 0,
                        GoalsPerMatch = totalGoals,
                        CardsPerMatch = totalCards,
                        CornersPerMatch = match.HomeCorners + match.AwayCorners,
                        MatchesThisSeason = 1
                    };
                }
                else
                {
                    float goalsPerMatch = (totalGoals + (oldStats.GoalsPerMatch * oldStats.MatchesThisSeason))/(oldStats.MatchesThisSeason + 1);
                    float cardsPerMatch = (totalCards + (oldStats.CardsPerMatch * oldStats.MatchesThisSeason)) / (oldStats.MatchesThisSeason + 1);
                    float cornersPerMatch = (totalCorners + (oldStats.CornersPerMatch *  oldStats.MatchesThisSeason))/(oldStats.MatchesThisSeason + 1);
                    return new LeagueStats()
                    {
                        CompetitionId = oldStats.CompetitionId,
                        SeasonId = oldStats.SeasonId,
                        OverOnePointFive = oldStats.OverOnePointFive + (totalGoals >= 2 ? 1 : 0),
                        OverTwoPointFive = oldStats.OverTwoPointFive + (totalGoals >= 3 ? 1 : 0),
                        BTTS = oldStats.BTTS + (btts ? 1 : 0),
                        GoalsPerMatch = goalsPerMatch,
                        CardsPerMatch = cardsPerMatch,
                        CornersPerMatch = cornersPerMatch,
                        MatchesThisSeason = oldStats.MatchesThisSeason + 1
                    };
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }





        private async Task<bool> ValidateHit(string competitionId)
        {
            try
            {
                string userId = _httpContext.GetUserId();
                PlanFeatures planFeatures = await _lsRepo.GetPlanFeatures(userId);
                List<LeagueStatsCount> countHistory = await _lsRepo.GetHitsRecordOfUser(userId);
                if (planFeatures is null || countHistory is null)
                    return false;
                foreach(LeagueStatsCount rec in countHistory)
                {
                    if (rec.LeagueId.Equals(competitionId))
                    {
                        return true;
                    }
                }
                if(countHistory.Count >= planFeatures.LeagueStatistics)
                {
                    return false;
                }
                LeagueStatsCount counter = new LeagueStatsCount()
                {
                    LeagueId = competitionId,
                    UserId = _httpContext.GetUserId()
                };
                return await _lsRepo.AddLeagueStatsCounter(counter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> UpdateStatsForLeague(string competitionId)
        {
            try
            {
                var competition = await _basicInfo.Competition(competitionId);
                List<RecentMatchResponseDto> seasonMatches = await _basicData.ScheduleAndResultsSeasonQuery();
                

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<LeagueStats> GetStatsOfLeagueFromApi(string leagueId, List<ApiSeason> allSeasons = null)
        {
            try
            {
                List<ApiCompetition> competition = await _basicInfo.Competition(leagueId);
                string currSeasonId = competition[0].CurrentSeasonId;
                if (string.IsNullOrEmpty(currSeasonId)) return null;
                List<ApiSeason> season = await _basicInfo.Season(currSeasonId);
                DateTime seasonStartDate = DateTimeOffset.FromUnixTimeSeconds(season[0].StartTime).UtcDateTime;
                bool oldSeason = false;
                if(seasonStartDate > DateTime.UtcNow)
                {
                    oldSeason = true;
                    allSeasons = allSeasons is null ? await _basicInfo.Season() : allSeasons;
                    List<ApiSeason> allSeasonsOfLeague = allSeasons.Where(seas => seas.CompetitionId.Equals(leagueId)).OrderByDescending(seas => seas.StartTime).ToList();
                    if(allSeasonsOfLeague is not null && allSeasonsOfLeague.Count >= 2)
                    {
                        season = new List<ApiSeason> { allSeasonsOfLeague[1] };
                        currSeasonId = season[0].Id;
                    }
                }

                LeagueStats stats = new LeagueStats();
                if (oldSeason)
                {
                    List<RecentMatchModel> matchesOfOldSeason = await _lsRepo.GetAllRecentMatchesOfSeason(currSeasonId);
                    stats = GetStatsOfOldSeason(matchesOfOldSeason, competition[0]);
                }
                else
                {
                    List<RecentMatchResponseDto> matchesOfSeason = await _basicData.ScheduleAndResultsSeasonQuery(currSeasonId);
                    matchesOfSeason = matchesOfSeason.Where(rm => rm.StatusId == 8).ToList();
                    stats = new LeagueStats();
                    int n = matchesOfSeason.Count;
                    foreach (var match in matchesOfSeason)
                    {
                        int goals = match.HomeScores[0] + match.AwayScores[0];
                        int corners = match.HomeScores[4] + match.AwayScores[4];
                        stats.BTTS = (match.HomeScores[0] > 0 && match.AwayScores[0] > 0) ? stats.BTTS + 1 : stats.BTTS;
                        stats.OverOnePointFive = goals >= 2 ? stats.OverOnePointFive + 1 : stats.OverOnePointFive;
                        stats.OverTwoPointFive = goals >= 3 ? stats.OverTwoPointFive + 1 : stats.OverTwoPointFive;
                        //stats.OverTwoPointFive = (float)(stats.OverTwoPointFive + (goals >= 3 ? 1 : 0)) / n * 100;
                        stats.OverThreePointFive = goals >= 4 ? stats.OverThreePointFive + 1 : stats.OverThreePointFive;
                        stats.AvgCardsRed += (match.HomeScores[2] + match.AwayScores[2]) / (float)n;
                        stats.AvgCardsYellow += (match.HomeScores[3] + match.AwayScores[3]) / (float)n;
                        stats.Draws = match.HomeScores[0] + match.HomeScores[5] + match.HomeScores[6] == match.AwayScores[0] + match.AwayScores[5] + match.AwayScores[6] ? stats.Draws + 1 : stats.Draws;
                        stats.GoalsPerMatch += ((float)goals) / n;
                        stats.CornersPerMatch += ((float)corners) / n;
                        stats.MatchesThisSeason++;
                    }

                    if (n > 0)
                    {
                        stats.BTTS = (int)(stats.BTTS * 100 / (float)n);
                        stats.OverOnePointFive = (int)(stats.OverOnePointFive * 100 / (float)n);
                        stats.OverTwoPointFive = (int)(stats.OverTwoPointFive * 100 / (float)n);
                        stats.OverThreePointFive = (int)(stats.OverThreePointFive * 100 / (float)n);
                        stats.Draws = (int)(stats.Draws * 100 / (float)n);
                    }
                }
                stats.CompetitionId = leagueId;
                stats.CompetitionLogo = competition[0].Logo;
                stats.SeasonId = currSeasonId;
                stats.SeasonStartDate = DateTimeOffset.FromUnixTimeSeconds(season[0].StartTime).DateTime;
                stats.SeasonEndDate = DateTimeOffset.FromUnixTimeSeconds(season[0].EndTime).DateTime;
                stats.UpdatedTime = DateTime.UtcNow;
                stats.Competition = await _lsRepo.GetCompetition(leagueId);
                stats.Competition.RecentMatches = null;
                stats.AvgGamesPlayed = stats.Competition.TeamCount == 0 ? 0 : stats.MatchesThisSeason/stats.Competition.TeamCount;
                return stats;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }


        private LeagueStats GetStatsOfOldSeason(List<RecentMatchModel> matches, ApiCompetition competition)
        {
            try
            {
                LeagueStats stats = new LeagueStats();
                int n = matches.Count;
                foreach (var match in matches)
                {
                    int goals = match.HomeGoals + match.AwayGoals;
                    int corners = match.HomeCorners + match.AwayCorners;

                    stats.BTTS = (match.HomeGoals > 0 && match.AwayGoals > 0) ? stats.BTTS + 1 : stats.BTTS;
                    stats.OverOnePointFive = goals >= 2 ? stats.OverOnePointFive + 1 : stats.OverOnePointFive;
                    stats.OverTwoPointFive = goals >= 3 ? stats.OverTwoPointFive + 1 : stats.OverTwoPointFive;
                    //stats.OverTwoPointFive = (float)(stats.OverTwoPointFive + (goals >= 3 ? 1 : 0)) / n * 100;
                    stats.OverThreePointFive = goals >= 4 ? stats.OverThreePointFive + 1 : stats.OverThreePointFive;
                    stats.AvgCardsRed += (match.HomeRedCards + match.AwayRedCards) / (float)n;
                    stats.AvgCardsYellow += (match.HomeYellowCards + match.AwayYellowCards) / (float)n;
                    stats.Draws = match.GameDrawn ? stats.Draws + 1 : stats.Draws;
                    stats.GoalsPerMatch += ((float)goals) / n;
                    stats.CornersPerMatch += ((float)corners) / n;
                    stats.MatchesThisSeason++;
                }

                if (n > 0)
                {
                    stats.BTTS = (int)(stats.BTTS * 100 / (float)n);
                    stats.OverOnePointFive = (int)(stats.OverOnePointFive * 100 / (float)n);
                    stats.OverTwoPointFive = (int)(stats.OverTwoPointFive * 100 / (float)n);
                    stats.OverThreePointFive = (int)(stats.OverThreePointFive * 100 / (float)n);
                    stats.Draws = (int)(stats.Draws * 100 / (float)n);
                }
                return stats;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }



        private async Task<List<string>> GetFavouriteTeamsOfUser(string userId)
        {
            try
            {
                ApplicationUser user = await _user.FindByIdAsync(userId);
                if (user == null)
                    return new List<string>();
                return user.FavouriteCompetition.Select(comp => comp.CompetitionId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<string>() ;
            }
        }

        public async Task<Result<List<LeagueStats>>> GetStatsOfLeagueMultiple(List<string> leagueIds)
        {
            try
            {
                List<LeagueStats> res = new List<LeagueStats>();

                foreach(var leagueId in leagueIds)
                {
                   
                    LeagueStats stat = await _lsRepo.GetLeagueStatsByLeagueId(leagueId);
                    res.Add(stat);
                   bool suc = stat is not null;
                }
                //if(!(await ValidateHit(leagueId)))
                //    return new Result<LeagueStats>(400, false, "Hit limit expired", null);

             return new Result<List<LeagueStats>>(200 , true, "", res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<List<LeagueStats>>(500, false, "Internal Server Erro", null);
            }
        }

        public async Task<Result<bool>> SetFavouriteLeagueStats(List<string> leagueIds)
        {
            try
            {
                List<FavouriteLeagueStat> favStats = new List<FavouriteLeagueStat>();
                string userId = _httpContext.GetUserId();
                leagueIds.ForEach(id => favStats.Add(new FavouriteLeagueStat() {
                    UserId = userId,
                    LeagueId = id
                }));

                bool added = await _lsRepo.SetFavouriteLeagueStats(favStats);
                return added ? Result<bool>.Success() : Result<bool>.InternalServerError();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError();
            }
        }

        public async Task<Result<List<string>>> GetFavouriteLeagueStatIds()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                List<FavouriteLeagueStat> favLeagueIds = await _lsRepo.GetFavouriteLeagueStats(userId);
                List<string> res = new List<string>();
                favLeagueIds.ForEach(fl => res.Add(fl.LeagueId));
                return Result<List<string>>.Success(item: res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<string>>.InternalServerError();
            }
        }

        public async Task<Result<List<LeagueStats>>> GetFavouriteLeagueStats()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                List<FavouriteLeagueStat> favLeagueIds = await _lsRepo.GetFavouriteLeagueStats(userId);
                if(favLeagueIds is null || favLeagueIds.Count == 0)
                {
                    return await GetTenLeagueStats();
                }
                List<string> res = new List<string>();
                favLeagueIds.ForEach(fl => res.Add(fl.LeagueId));
                return await GetMultipleLeagueStats(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<List<LeagueStats>>.InternalServerError();
            }
        }

        private async Task<LeagueStats> GetLeagueStatFromSportsApi(string leagueId)
        {
            try
            {
                List<ApiCompetition> competition = await _basicInfo.Competition(leagueId);
                string currSeasonId = competition[0].CurrentSeasonId;
                if (string.IsNullOrEmpty(currSeasonId)) return null;
                LeagueStats stats = new LeagueStats();
                List<RecentMatchResponseDto> matchesOfSeason = await _basicData.ScheduleAndResultsSeasonQuery(currSeasonId);
                matchesOfSeason = matchesOfSeason.Where(rm => rm.StatusId == 8).ToList();
                stats = new LeagueStats();
                int n = matchesOfSeason.Count;
                List<ApiSeason> season = await _basicInfo.Season(currSeasonId);
                foreach (var match in matchesOfSeason)
                {
                    int goals = match.HomeScores[0] + match.AwayScores[0];
                    int corners = match.HomeScores[4] + match.AwayScores[4];
                    stats.BTTS = (match.HomeScores[0] > 0 && match.AwayScores[0] > 0) ? stats.BTTS + 1 : stats.BTTS;
                    stats.OverOnePointFive = goals >= 2 ? stats.OverOnePointFive + 1 : stats.OverOnePointFive;
                    stats.OverTwoPointFive = goals >= 3 ? stats.OverTwoPointFive + 1 : stats.OverTwoPointFive;
                    //stats.OverTwoPointFive = (float)(stats.OverTwoPointFive + (goals >= 3 ? 1 : 0)) / n * 100;
                    stats.OverThreePointFive = goals >= 4 ? stats.OverThreePointFive + 1 : stats.OverThreePointFive;
                    stats.AvgCardsRed += (match.HomeScores[2] + match.AwayScores[2]) / (float)n;
                    stats.AvgCardsYellow += (match.HomeScores[3] + match.AwayScores[3]) / (float)n;
                    stats.Draws = match.HomeScores[0] + match.HomeScores[5] + match.HomeScores[6] == match.AwayScores[0] + match.AwayScores[5] + match.AwayScores[6] ? stats.Draws + 1 : stats.Draws;
                    stats.GoalsPerMatch += ((float)goals) / n;
                    stats.CornersPerMatch += ((float)corners) / n;
                    stats.MatchesThisSeason++;
                }

                if (n > 0)
                {
                    stats.BTTS = (int)(stats.BTTS * 100 / (float)n);
                    stats.OverOnePointFive = (int)(stats.OverOnePointFive * 100 / (float)n);
                    stats.OverTwoPointFive = (int)(stats.OverTwoPointFive * 100 / (float)n);
                    stats.OverThreePointFive = (int)(stats.OverThreePointFive * 100 / (float)n);
                    stats.Draws = (int)(stats.Draws * 100 / (float)n);
                }
                stats.CompetitionId = leagueId;
                stats.CompetitionLogo = competition[0].Logo;
                stats.SeasonId = currSeasonId;
                stats.SeasonStartDate = DateTimeOffset.FromUnixTimeSeconds(season[0].StartTime).DateTime;
                stats.SeasonEndDate = DateTimeOffset.FromUnixTimeSeconds(season[0].EndTime).DateTime;
                stats.UpdatedTime = DateTime.UtcNow;
                stats.Competition = await _lsRepo.GetCompetition(leagueId);
                stats.Competition.RecentMatches = null;
                stats.AvgGamesPlayed = stats.Competition.TeamCount == 0 ? 0 : stats.MatchesThisSeason / stats.Competition.TeamCount;
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        private async Task<LeagueStats> GetLeagueStatFromDb(string seasonId)
        {
            try
            {
                List<RecentMatchModel> matches = await _lsRepo.GetAllRecentMatchesOfSeason(seasonId);
                //List<RecentMatchModel> matches, ApiCompetition competition
                LeagueStats stats = new LeagueStats();
                int n = matches.Count;
                foreach (var match in matches)
                {
                    int goals = match.HomeGoals + match.AwayGoals;
                    int corners = match.HomeCorners + match.AwayCorners;
                    stats.BTTS = (match.HomeGoals > 0 && match.AwayGoals > 0) ? stats.BTTS + 1 : stats.BTTS;
                    stats.OverOnePointFive = goals >= 2 ? stats.OverOnePointFive + 1 : stats.OverOnePointFive;
                    stats.OverTwoPointFive = goals >= 3 ? stats.OverTwoPointFive + 1 : stats.OverTwoPointFive;
                    //stats.OverTwoPointFive = (float)(stats.OverTwoPointFive + (goals >= 3 ? 1 : 0)) / n * 100;
                    stats.OverThreePointFive = goals >= 4 ? stats.OverThreePointFive + 1 : stats.OverThreePointFive;
                    stats.AvgCardsRed += (match.HomeRedCards + match.AwayRedCards) / (float)n;
                    stats.AvgCardsYellow += (match.HomeYellowCards + match.AwayYellowCards) / (float)n;
                    stats.Draws = match.GameDrawn ? stats.Draws + 1 : stats.Draws;
                    stats.GoalsPerMatch += ((float)goals) / n;
                    stats.CornersPerMatch += ((float)corners) / n;
                    stats.MatchesThisSeason++;
                }

                if (n > 0)
                {
                    stats.BTTS = (int)(stats.BTTS * 100 / (float)n);
                    stats.OverOnePointFive = (int)(stats.OverOnePointFive * 100 / (float)n);
                    stats.OverTwoPointFive = (int)(stats.OverTwoPointFive * 100 / (float)n);
                    stats.OverThreePointFive = (int)(stats.OverThreePointFive * 100 / (float)n);
                    stats.Draws = (int)(stats.Draws * 100 / (float)n);
                }
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

    }
}