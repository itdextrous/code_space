
using InPlayWise.Core.IServices;
using InPlayWise.Data.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InPlayWise.Core.Services
{
    public class ShocksService : IShocksService
    {
        private readonly IShocksRepository _repo;
        private readonly IHttpContextService _httpContext;
        private readonly ILogger<ShocksService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShocksService(IShocksRepository repo, IHttpContextService httpContext, ILogger<ShocksService> logger, UserManager<ApplicationUser> userManager )
        {
            _repo = repo;
            _httpContext = httpContext;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<MatchShocksResponseDto>> GetShockFactsOfMatch(string matchId)
        {
            try
            {
                bool validated = await ValidateHit(matchId);
                return new Result<MatchShocksResponseDto>(200, true, "validated", new MatchShocksResponseDto());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new Result<MatchShocksResponseDto>(500, false, "Internal server error", new MatchShocksResponseDto());
            }
        }


        private async Task<bool> ValidateHit(string matchId)
        {
            try
            {
                string userId = _httpContext.GetUserId();
                PlanFeatures planFeatures = await _repo.GetPlanFeatures(userId);
                ShockCounter scCount = new ShockCounter()
                {
                    Matchid = matchId,
                    UserId = _httpContext.GetUserId(),
                    Count = 1
                };
                int hits = await _repo.AddCounterIfDoesntExistsAndReturnHits(scCount);
                if (hits == -1)
                    return false;
                else if(hits  > planFeatures.ShockDetectors)
                {
                    return false;
                }
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

    }
}