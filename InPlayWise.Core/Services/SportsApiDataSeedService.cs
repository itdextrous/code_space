using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Core.Mappings;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Logging;

namespace InPlayWise.Core.Services
{
    public class SportsApiDataSeedService : ISportsApiDataSeedService
	{
		private readonly ISportsApiDataSeedRepository _repo;
		private readonly ILogger<SportsApiDataSeedService> _logger;
		private readonly MatchInMemoryService _inMemory;
		private readonly IBasicInfoServices _basicInfo;
		private readonly IBasicDataServices _basicData;

		public SportsApiDataSeedService(ISportsApiDataSeedRepository repo, ILogger<SportsApiDataSeedService> logger, MatchInMemoryService inMemory, IBasicInfoServices basicInfo, IBasicDataServices basicData)
		{
			_repo = repo;
			_logger = logger;
			_inMemory = inMemory;
			_basicInfo = basicInfo;
			_basicData = basicData;
		}

		public async Task<bool> SeedCategory()
		{
			try
			{
				List<ApiCategory> apiCategories = await _basicInfo.Category();
				List<Category> categories = new List<Category>();
				List<string> existingCategories = await _repo.GetAllCategoriesId();
				foreach (ApiCategory apiCategory in apiCategories)
				{
					if (!existingCategories.Contains(apiCategory.Id))
					{
						categories.Add(MappingService.ApiCategoryToEntity(apiCategory));
					}
				}
				bool saved = await _repo.AddCategories(categories);
				return saved;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}

		public async Task<bool> SeedCountry()
		{
			try
			{
				List<ApiCountry> apiCountries = await _basicInfo.Country();
				List<Country> countries = new List<Country>();
				List<string> existingCountries = await _repo.GetAllContriesId();
				foreach(ApiCountry apiCountry in apiCountries)
				{
					if (!existingCountries.Contains(apiCountry.Id))
					{
						countries.Add(MappingService.ApiCountryToEntity(apiCountry));
					}
				}
				bool saved = await _repo.AddCountries(countries);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}


		public async Task<bool> SeedSeason()
		{
			try
			{
				List<ApiSeason> apiSeasons = await _basicInfo.Season();
				List<Season> seasons = new List<Season>();
				apiSeasons.Select(apiSeas => MappingService.ApiSeasonToEntity(apiSeas)).ToList().ForEach(seas => seasons.Add(seas));
				bool saved = await _repo.AddSeasons(seasons);
				return saved;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		//public async Task<bool> SeedTeam()
		//{
		//	try
		//	{
		//		List<ApiTeam> apiTeams = await _basicInfo.Team();
		//		List<Team> teams = new List<Team>();
		//		apiTeams.Select(apiTeam => MappingService.ApiTeamToEntity(apiTeam)).ToList().ForEach(team => teams.Add(team));
		//		bool saved = await _repo.AddTeams(teams);
		//		return saved;
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine(ex);
		//		return false;
		//	}

		//}


		public async Task<bool> SeedUpcomingMatches()
		{
			try
			{
				List<ApiRecentMatch> apiMatches = await _basicData.ScheduleAndResultsDateQuery(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
				apiMatches = apiMatches.OrderBy(match => match.MatchTime).Take(300).ToList();
				List<string> existingMatches = (await _repo.GetAllUpcomingMatches()).Select(match => match.Id).ToList();
				List<UpcomingMatch> matches = new List<UpcomingMatch>();
				foreach(var apiMatch in apiMatches)
				{
					if (existingMatches.Contains(apiMatch.Id)) continue;
					if (DateTimeOffset.FromUnixTimeSeconds(apiMatch.MatchTime) < DateTime.UtcNow) continue;
					UpcomingMatch match = MappingService.ApiRecentMatchToUpcomingEntity(apiMatch);
					match.HomeTeamLastGame = GetLastMatchDateOfTeam(match.HomeTeamId);
					match.AwayTeamLastGame = GetLastMatchDateOfTeam(match.AwayTeamId);
					if(!string.IsNullOrEmpty(apiMatch.Round.StageId))
						match.StageName = (await _basicInfo.Stage(apiMatch.Round.StageId))[0].Name;
					matches.Add(match);
				}
				bool saved = await _repo.AddUpcomingMatches(matches);
				matches = await _repo.GetAllUpcomingMatches();
				_inMemory.SetUpcomingMatches(matches);
				return true;
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}


		private bool StringExistsInList(List<string> li, string target)
		{
			try
			{
				if (li is null) return true;
				int low = 0, high = li.Count - 1;
				while (low <= high)
				{
					int mid = (low + high) / 2;
					string midValue = li[mid];
					int compareResult = string.Compare(midValue, target);
					if (compareResult == 0)
						return true;
					else if (compareResult < 0)
						low = mid + 1;
					else
						high = mid - 1;
				}
				return false;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return true;
			}
		}

		public async Task<bool> SeedTeamCount()
		{
			// seed team count can't be accomodated in seed team or competition as it needs
			// to be done after successful seeding of competitions and then teams
			try
			{ 
				List<ApiTeam> allTeams = await _basicInfo.Team();
				
				
				List<Competition> dbComps = await _repo.GetAllLeaguesWithoutTeamCount();
				foreach(Competition comp in dbComps)
				{
					int count = allTeams.Where(t => t.CompetitionId.Equals(comp.Id)).ToList().Count();
					comp.TeamCount = count;
				}
				bool updated = await _repo.UpdateTeamCount(dbComps);
				return updated;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return false;
			}
		}

		public async Task<bool> SeedCompetitions()
        {
            try
            {
				List<string> existingCompetitions = await _repo.GetAllCompetitionId();
				List<ApiCompetition> apiCompetitions = await _basicInfo.Competition();
				List<Competition> competitionsToSave = new List<Competition>();

				foreach(ApiCompetition apiComp in apiCompetitions)
				{
					string id = apiComp.Id;
					if (string.IsNullOrEmpty(apiComp.Name) || StringExistsInList(existingCompetitions, id))
						continue;
					Competition comp = MappingService.ApiCompetitionToCompetitionModel(apiComp);
					competitionsToSave.Add(comp);
				}
				bool saved = await _repo.SaveCompetitionList(competitionsToSave);
                if (saved)
                {
					List<Competition> competitionsToUpdate = new List<Competition>();
                    List<string> incompleteCompeitionIds = await _repo.GetCompetitionsWithIncompleteInfo();
                    foreach (string compId in incompleteCompeitionIds)
                    {
						ApiCompetition apiComp = apiCompetitions.SingleOrDefault(cm => cm.Id.Equals(compId));
						if(apiComp is null) { continue; }
						Competition com = MappingService.ApiCompetitionToEntity(apiComp);
						competitionsToUpdate.Add(com);
                    }
					await _repo.UpdateMultipleCompeitions(competitionsToUpdate);
                }
                return saved;

            }
            catch (Exception ex)
            {
				_logger.LogError(ex.ToString());
				return false;
            }
        }

        public async Task<bool> SeedTeams()
        {
            try
            {
                List<string> ExistingTeams = await _repo.GetAllTeamsId();
				List<ApiTeam> apiTeams = await _basicInfo.Team();
				List<Team> teamsToSave = new List<Team>();
				foreach (var apiTeam in apiTeams)
				{
					string id = apiTeam.Id;
					if (string.IsNullOrEmpty(apiTeam.Name) || StringExistsInList(ExistingTeams, id))
						continue;
					Team tm = MappingService.ApiTeamToEntity(apiTeam);
					teamsToSave.Add(tm);
				}
				bool saved = await _repo.SaveTeamsList(teamsToSave);
				if (saved)
				{
					List<Team> teamsToUpdate = new List<Team>();
					List<string> incompleteTeamIds = await _repo.GetTeamsWithIncompleteInfo();
                    foreach (string teamId in incompleteTeamIds)
                    {
						Team tm = MappingService.ApiTeamToEntity(apiTeams.SingleOrDefault(tm => tm.Id.Equals(teamId)));
						teamsToUpdate.Add(tm);
                    }
					await _repo.UpdateMultipleTeams(teamsToUpdate);
                }
				return saved;
            }
            catch (Exception ex)
            {
				_logger.LogError(ex.ToString());
				return false;
            }
        }



		private DateTime GetLastMatchDateOfTeam(string teamId)
		{
			try
			{
                List<RecentMatchModel> matches;
                if (!_inMemory.GetPastMatchesHash().TryGetValue(teamId, out matches) || matches == null || !matches.Any())
                {
                    return DateTime.MinValue;
                }

                return matches
                    .OrderByDescending(match => match.MatchStartTimeOfficial)
                    .FirstOrDefault()?
                    .MatchStartTimeOfficial ?? DateTime.MinValue;
                //            List<RecentMatchModel> matches = _inMemory.GetRecentMatches();
                //            List<RecentMatchModel> result = new List<RecentMatchModel>();
                //DateTime dt = new DateTime();
                //            foreach (var match in matches.ToList())
                //            {
                //                bool condition = string.IsNullOrEmpty(match.HomeTeamId) || string.IsNullOrEmpty(match.AwayTeamId) ;
                //                if (condition)
                //                    continue;
                //                if (match.HomeTeamId.Equals(teamId) || match.AwayTeamId.Equals(teamId))
                //                {
                //                    if(match.MatchStartTimeOfficial > dt)
                //		{
                //			dt = match.MatchStartTimeOfficial;
                //		}
                //                }
                //            }
                //return dt;
            }
            catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
				return new DateTime();
			}
		}


    }
}
