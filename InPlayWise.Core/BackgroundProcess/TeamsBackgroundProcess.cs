using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Services;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.BackgroundProcess
{
    public class TeamsBackgroundProcess : ITeamsBackgroundProcess
    {
        private readonly ITeamsRepository _teamsRepo;
        private readonly ILogger<TeamsBackgroundProcess> _logger;
        private readonly MatchInMemoryService _inMemory;
        public TeamsBackgroundProcess(ITeamsRepository teamsRepo, ILogger<TeamsBackgroundProcess> logger, MatchInMemoryService inMemory, IBasicDataServices basicDataServices, IConfiguration configuration)
        {
            _teamsRepo = teamsRepo;
            _logger = logger;
            _inMemory = inMemory;
        }

        public async Task<bool> SeedCompetitionsInMemory()
        {
            try
            {
                List<Competition> dbCompetitions = await _teamsRepo.GetAllCompetions();
                _inMemory.SetCompetitions(dbCompetitions);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> SeedTeamsInMemory()
        {
            try
            {

                List<Team> dbTeams = await _teamsRepo.GetAllTeams();
                _inMemory.SetTeams(dbTeams);
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
