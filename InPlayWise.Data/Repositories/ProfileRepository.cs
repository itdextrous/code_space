using InPlayWise.Common.Enums;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
    public class ProfileRepository : IProfileRepository
    {

        private readonly AppDbContext _db;
        private readonly ILogger<ProfileRepository> _logger;

        public ProfileRepository(AppDbContext context, ILogger<ProfileRepository> logger) {
            _db = context;
            _logger = logger;
        }

        public async Task<bool> AddFavouriteTeam(string userId, List<string> teamIds)
        {
            try
            {
                var newFavouriteTeams = teamIds.Select(teamId => new FavouriteTeams
                {
                    Id = Guid.NewGuid(),
                    TeamId = teamId,
                    UserId = userId
                }).ToList();

                await _db.FavouriteTeams.AddRangeAsync(newFavouriteTeams);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> AddFavouriteCompetition(string userId, List<string> leagueIds)
        {
            try
            {
                List<FavouriteCompetitions> favComps = new List<FavouriteCompetitions>();
                foreach (string leagueId in leagueIds)
                {
                    FavouriteCompetitions fvcm = new FavouriteCompetitions()
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        CompetitionId = leagueId
                    };
                    var exists = await _db.FavouriteCompetitions.SingleOrDefaultAsync(fvcm => fvcm.UserId.Equals(leagueId) && fvcm.CompetitionId.Equals(leagueId));
                    if (exists is null)
                        favComps.Add(fvcm);
                }
                await _db.FavouriteCompetitions.AddRangeAsync(favComps);
                await _db.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        public async Task<bool> CreateProfile(string userId)
        {
            try
            {
                UserProfile profile = new UserProfile()
                {
                    UserId = userId
                };
                await _db.Profiles.AddAsync(profile);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> SetDarkTheme(string userId)
        {
            try
            {
                var profile = await _db.Profiles.FindAsync(userId);
                if (profile is null)
                {
                    return false;
                }
                profile.ThemeDark = true;
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> SetLightTheme(string userId)
        {
            try
            {
                var profile = await _db.Profiles.FindAsync(userId);
                if (profile == null)
                {
                    return false;
                }
                profile.ThemeDark = false;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> ThemeDark(string userId)
        {
            try
            {
                var profile = await _db.Profiles.FindAsync(userId);
                return profile.ThemeDark;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<Team>> GetFavouriteTeams(string userId)
        {
            try
            {
                var favTeams =  await _db.FavouriteTeams.Where(ft => ft.UserId.Equals(userId)).ToListAsync();
                List<Team> teams = new List<Team>();
                foreach(var favTeam in favTeams)
                {
                    Team tm = await _db.Team.FindAsync(favTeam.TeamId);
                    teams.Add(tm);
                }
                return teams;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Competition>> GetFavouriteCompetitions(string userId)
        {
            try
            {
                List<FavouriteCompetitions> favComps = await _db.FavouriteCompetitions.Where(fc => fc.UserId.Equals(userId)).ToListAsync();
                List<Competition> competitions = new List<Competition>();
                foreach (FavouriteCompetitions favComp in favComps)
                {
                    Competition cm = await _db.Competitions.FindAsync(favComp.CompetitionId);
                    competitions.Add(cm);
                }
                return competitions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }




        // These are feature counter services

        public async Task<PlanFeatures> GetPlanFeatures(string userId)
        {
            try
            {
                Guid productId = (await _db.Subscriptions.SingleOrDefaultAsync(sub => sub.UserId.Equals(userId))).ProductId;
                PlanFeatures feature = await _db.PlanFeatures.SingleOrDefaultAsync(ft => ft.ProductId.Equals(productId));
                return feature;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }


        public async Task<List<PredictionCounter>> GetPredictionRequestsOfUser(string userId)
        {
            try
            {
                return await _db.PredictionCounters.Where(pc => pc.UserId.Equals(userId)).ToListAsync();
                
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<int> AccumulatorGeneratorCounterOfUser(string userId)
        {
            try
            {
                return -1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return -1;
            }
        }

        public async Task<List<ShockCounter>> GetShockCounterRequestOfUser(string userId)
        {
            try
            {
                return await _db.ShockCounter.Where(sc => sc.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<CleverLabelsCounter>> GetCleverLabelsRequestOfUser(string userId)
        {
            try
            {
                return await _db.CleverLabelsCounters.Where(clc => clc.UserId.Equals(userId)).ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<int> GetHistoryOfAccumulatorsRequestOfUser(string userId)
        {
            try
            {
                return -1;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return -1;
            }
        }

        public async Task<int> GetHistoryOfWiseProHedgeOfUser(string userId)
        {
            try
            {
                return -1;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return -1;
            }
        }

        public async Task<List<LeagueStatsCount>> GetLeagueStatsRequestOfUser(string userId)
        {
            try
            {
                return await _db.LeagueStatsCounter.Where(lsc => lsc.UserId.Equals(userId)).ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<bool> DeleteFavouriteCompetitions(string userId, List<string> competitionIds)
        {
            try
            {
                List<FavouriteCompetitions> comps = await _db.FavouriteCompetitions.Where(comp => comp.UserId.Equals(userId) && competitionIds.Contains(comp.CompetitionId)).ToListAsync();
                _db.FavouriteCompetitions.RemoveRange(comps);
                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteFavouriteTeams(string userId, List<string> teamIds)
        {
            try
            {
                List<FavouriteTeams> teams = await _db.FavouriteTeams.Where(team => team.UserId.Equals(userId) && teamIds.Contains(team.TeamId)).ToListAsync();
                _db.FavouriteTeams.RemoveRange(teams);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public bool SetCountry(int index)
        {
            return true;
        }

        public async Task<List<string>> GetFavouriteTeamIds(string userId)
        {
            try
            {
                // Query the database to get the list of team IDs for the given user ID
                var teamIds = await _db.FavouriteTeams
                    .Where(fvtm => fvtm.UserId == userId)
                    .Select(fvtm => fvtm.TeamId)
                    .ToListAsync();

                return teamIds;
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"An error occurred while fetching favourite team IDs: {ex.Message}");
                throw;
            }
        }
    }
}
