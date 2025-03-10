using AutoMapper;
using InPlayWise.Common.DTO;
using InPlayWise.Common.DTO.CachedDto;
using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace InPlayWise.Core.BackgroundProcess
{
    public class UserDataBackgroundProcess : IUserDataBackgroundProcess
    {
        private readonly IConfiguration _config;
        private readonly IAccumulaterRepository _accumulatorRepo ;
        private readonly IHttpContextService _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserDataBackgroundProcess> _logger;
        private readonly IMapper _mapper;
        private readonly MatchInMemoryService _inMemory;

        public UserDataBackgroundProcess(IAccumulaterRepository repo, IHttpContextService httpContext, UserManager<ApplicationUser> userManager, IConfiguration config, ILogger<UserDataBackgroundProcess> logger, IMapper mapper, MatchInMemoryService inMemory)
        {
            _accumulatorRepo = repo;
            _httpContext = httpContext;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _inMemory = inMemory;
        }

        public async Task<bool> UpdateUserAccumulators(string userId)
        {
            try
            {
                List<List<Accumulator>> savedAccumulators = await _accumulatorRepo.GetSavedAccumulatorsAsync(userId);
                List<List<OpportunityCachedDto>> mappedList = savedAccumulators
                    .Select(innerList => _mapper.Map<List<OpportunityCachedDto>>(innerList))
                    .ToList();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            return false;
            }
        }
    }
}
