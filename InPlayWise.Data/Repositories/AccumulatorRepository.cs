using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Data.Repositories
{
    public class AccumulatorRepository : IAccumulaterRepository
    {

        private readonly AppDbContext _db;
        private readonly ILogger<AccumulatorRepository> _logger;


        public AccumulatorRepository(AppDbContext db, ILogger<AccumulatorRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<List<Accumulator>>> GetSavedAccumulatorsAsync(string userId)
        {
            try
            {
                // Query the database to group accumulators by group ID for the specified user
                var groupedAccumulators = await _db.Accumulaters
                    .Where(a => a.UserId == userId)
                    .GroupBy(a => a.GroupId)
                    .OrderByDescending(g => g.Max(a => a.SavedTime)) // Assuming SavedTime exists and is relevant for ordering
                    .Select(g => g.OrderByDescending(a => a.ConfidencyLevel).ToList()) // Sort within each group by ConfidencyLevel
                    .ToListAsync();

                return groupedAccumulators;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving saved accumulators for user {UserId}.", userId);
                return null;
            }
        }


        

        public async Task<bool> SaveAccumulatorsAsync(List<Accumulator> accumulators)
        {
            try
            {
                string groupId = Guid.NewGuid().ToString();

                DateTime currentTime = DateTime.UtcNow;
                

                // Set the group ID and saved time for each accumulator
                foreach (var accumulator in accumulators)
                {
                    accumulator.GroupId = groupId;
                    accumulator.SavedTime = currentTime;
                    var matchMinute = await _db.LiveMatches
                                     .Where(lm => lm.MatchId == accumulator.MatchId)
                                     .Select(lm =>  lm.MatchMinutes)
                                     .FirstOrDefaultAsync();
                    accumulator.OppPlaced = matchMinute;
                }

                await _db.Accumulaters.AddRangeAsync(accumulators);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving accumulators");
                return false;
            }
        }

        public async Task<bool> DeleteAccumulatorAsync(string groupId)
        {
            try
            {
                // Retrieve accumulators associated with the specified group ID asynchronously
                var accumulatorsToDelete = await _db.Accumulaters
                    .Where(a => a.GroupId == groupId)
                    .ToListAsync();

                // Check if any accumulators were found for the group ID
                if (accumulatorsToDelete.Any())
                {
                    // Remove all accumulators associated with the specified group ID
                    _db.Accumulaters.RemoveRange(accumulatorsToDelete);

                    // Save changes to the database asynchronously
                    await _db.SaveChangesAsync();

                    // Return true to indicate successful deletion
                    return true;
                }

                // If no accumulators found for the group ID, return false
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting accumulators for group ID {GroupId}.", groupId);
                return false;
            }

           
        }

        public async Task<int> GetAccumulatorHistoryLimit(string productId)
        {
            try
            {
                // Retrieve HistoryOfAccumulater value from PlanFeatures table based on ProductId
                PlanFeatures planFeature =  _db.PlanFeatures.FirstOrDefault(pf => pf.ProductId.ToString() == productId);

                if (planFeature != null)
                {
                    return planFeature.HistoryOfAccumulators;
                }
                else
                {
                    return 0; // If no record found for the productId
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception as needed
                Console.WriteLine($"Error retrieving HistoryOfAccumulater: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetGeneratedAccumulatorLimit(string productId)
        {
            try
            {
                // Retrieve HistoryOfAccumulater value from PlanFeatures table based on ProductId
                var planFeature = _db.PlanFeatures.FirstOrDefault(pf => pf.ProductId.ToString() == productId);

                if (planFeature != null)
                {
                    return planFeature.AccumulatorGenerators;
                }
                else
                {
                    return 0; // If no record found for the productId
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception as needed
                Console.WriteLine($"Error retrieving HistoryOfAccumulater: {ex.Message}");
                throw;
            }


        }

        public async  Task<List<Accumulator>> GetAllOpps(string matchId)
        {
            try
            {
                var opps = await _db.Accumulaters
                    .Where(acc => acc.MatchId == matchId)
                    .ToListAsync();

                return opps;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching opportunities for MatchId: {matchId}");
                return null; 
            }
        }

        public async Task<bool> UpdateAccumulator(List<Accumulator> accumulaters)
        {
            try
            {
                 _db.Accumulaters.UpdateRange(accumulaters);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating accumulators.");
                return false;
            }
        }

        public async Task<bool> CheckIfAccumulatorsExistAsync(List<OpportunityDto> opportunityDtos, string userId)
        {
            //if (opportunityDtos == null || !opportunityDtos.Any() || string.IsNullOrWhiteSpace(userId))
            //{
            //    _logger.LogError("Invalid input to CheckIfAccumulatorsExistAsync");
            //    return false;
            //}

            try
            {
                var matchIds = opportunityDtos.Select(dto => dto.MatchId.Trim().ToLower()).ToList();
                var opportunityNames = opportunityDtos.Select(dto => dto.OpportunityName.Trim().ToLower()).ToList();

                var matchingAccumulators = await _db.Accumulaters
                    .Where(a => matchIds.Contains(a.MatchId.Trim().ToLower()) &&
                                opportunityNames.Contains(a.OpportunityName.Trim().ToLower()) &&
                                a.UserId == userId)
                    .ToListAsync();

                var groupIds = matchingAccumulators.Select(a => a.GroupId).Distinct().ToList();
                if (groupIds.Count != 1)
                {
                    _logger.LogWarning("GroupId inconsistency detected. GroupIds: {GroupIds}", string.Join(",", groupIds));
                    return false;
                    //return true; p0
                }

                if (matchingAccumulators.Count != opportunityDtos.Count)
                {
                    _logger.LogWarning("Count mismatch. Matching Accumulators: {Count}, DTOs: {DtoCount}",
                        matchingAccumulators.Count, opportunityDtos.Count);
                    return false;
                }

                var matchedKeys = matchingAccumulators
                    .Select(a => new { MatchId = a.MatchId.Trim().ToLower(), OpportunityName = a.OpportunityName.Trim().ToLower() })
                    .ToHashSet();

                var dtoKeys = opportunityDtos
                    .Select(dto => new { MatchId = dto.MatchId.Trim().ToLower(), OpportunityName = dto.OpportunityName.Trim().ToLower() })
                    .ToHashSet();

                return matchedKeys.SetEquals(dtoKeys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking accumulators");
                throw;
            }
        }

        public async Task<int> RawSqlExecute(string query)
        {
            try
            {
                return await _db.Database.ExecuteSqlRawAsync(query);
            }
            catch (Exception ex) {
                return -1;
            }
        }

        public async Task<List<List<Accumulator>>> GetAccumulatorGroupsByUserId(string userId)
        {
            try
            {
                return await _db.Accumulaters.Where(acc => acc.UserId.Equals(userId))
                            .GroupBy(acc => acc.GroupId) 
                            .Select(group => group.ToList()) 
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
