using InPlayWise.Common.Constants;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.Services
{
	public class CompetitionService : ICompetitionService
	{

		private readonly ICompetitionRepository _compRepo;
        private readonly IBasicInfoServices _basicInfoServices;
        public CompetitionService(ICompetitionRepository comp , IBasicInfoServices basicInfoServices) {
			_compRepo = comp;
			_basicInfoServices = basicInfoServices;
		}

		public async Task<Result<List<Competition>>> GetCompetitionByCountry(string name)
		{
			try
			{
				if(name.Count() < 3)
					return new Result<List<Competition>> { IsSuccess = false, Message = "Search with min 3 characters" };
				List<Competition> comp = await _compRepo.GetByCountry(name);
				return new Result<List<Competition>>()
				{
					Items = comp,
					IsSuccess = true,
					Message = "These are the competition",
					StatusCode = 200
				};
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<Competition>>()
				{
					IsSuccess = false,
					Message = "Internal Server error",
					StatusCode = 500
				};
			}
		}

		public async Task<Result<List<Competition>>> GetCompetitionByName(string name)
		{
			try
			{
				if (name.Count() < 3)
					return new Result<List<Competition>> { IsSuccess = false, Message = "Search with min 3 characters" };
				List<Competition> comp = await _compRepo.GetByName(name);
				return new Result<List<Competition>>()
				{
					Items = comp,
					IsSuccess = true,
					Message = "These are the competition",
					StatusCode = 200
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<Competition>>()
				{
					IsSuccess = false,
					Message = "Internal Server error",
					StatusCode = 500
				};
			}
		}

		public async Task<Result<List<Competition>>> GetFiftyCompetiton()
		{
			try
			{
				List<Competition> comp = await _compRepo.GetFiftyCompetition();
				return new Result<List<Competition>>()
				{
					Items = comp,
					IsSuccess = true,
					Message = "These are the competition",
					StatusCode = 200
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Result<List<Competition>>()
				{
					IsSuccess = false,
					Message = "Internal Server error",
					StatusCode = 500
				};
			}

		}
		public async Task<bool> AddCategory()
		{
            try
            {

				bool categoriesSeeded = await SeedCategory();

				List<ApiCompetition> apiCompetitions =  await _basicInfoServices.Competition();

				List<CompetionCategory> categories = await _compRepo.GetAllCategories();

				foreach(var apiComp in apiCompetitions)
				{
                    Competition compDb = await _compRepo.GetCompetitionById(apiComp.Id);
					if (compDb is null) continue;

                    CompetionCategory? CatogoryType = null; 

                    switch (apiComp.Type)
                    {
                        case 1:
                            CatogoryType = categories.FirstOrDefault(ct => ct.Name.Equals("Domestic"));
                            break;
                        case 2:
                            CatogoryType = categories.FirstOrDefault(ct => ct.Name.Equals("Cup"));
                            break;
                        case 3:
                            CatogoryType = categories.FirstOrDefault(ct => ct.Name.Equals("Friendly"));
                            break;
                        default:
                            // Handle default case
                            break;
                    }

                    if (CatogoryType != null)
                    {
                        if (compDb.CompetitionCategories is null)
                        {
                            compDb.CompetitionCategories = new List<CompetionCategory>();
                        }
                        compDb.CompetitionCategories.Add(CatogoryType);
                        await _compRepo.UpdateCompetition(compDb);
                    }
                }

				return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
				return false;
            }
        }

        private async Task<bool> SeedCategory()
        {
            try
            {

				List<string> categories = new List<string>()
				{
					"Domestic", "Cup", "Champions", "Continental", "Intercontinental", "Country", "Friendly", "Qualifications", "UnderXX", "Women"
				};

				List<CompetionCategory> allComps = await _compRepo.GetAllCategories();
				List<string> catNames = allComps.Select(cc => cc.Name).ToList();

				foreach(string catToSeed in categories)
				{
					if (catNames.Contains(catToSeed)) continue;

					CompetionCategory cc = new CompetionCategory()
					{
						Id = Guid.NewGuid(),
						Name = catToSeed
					};
					bool added = await _compRepo.AddCategory(cc);
				}
				return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<Result<List<Competition>>> GetCompetition(string id, bool isLeague)
        {
			try
			{
				if (isLeague)
				{
                    if (id is null)
                    {
                        List<Competition> allComps = await _compRepo.GetAllLeagues();
                        if (allComps is not null && allComps.Count != 0)
                            return Result<List<Competition>>.Success("", allComps);
                        return Result<List<Competition>>.NotFound();
                    }
                    if (id.Count() < 3)
                        return new Result<List<Competition>> { IsSuccess = false, Message = "Invalid Id" };
                    List<Competition> leagueRes = new List<Competition> { await _compRepo.GetCompetitionById(id) }.Where(comp => comp.LeagueLevel != -1).ToList();
                    if (leagueRes is not null && leagueRes.Count != 0)
                        return Result<List<Competition>>.Success("", leagueRes);
                    return Result<List<Competition>>.NotFound();
                }
				if(id is null)
				{
					List<Competition> allComps = await _compRepo.GetAllCompetition();
					if (allComps is not null && allComps.Count != 0)
						return Result<List<Competition>>.Success("", allComps);
					return Result<List<Competition>>.NotFound();
				}
                if (id.Count() < 3)
                    return new Result<List<Competition>> { IsSuccess = false, Message = "Invalid Id" };
                List<Competition> res = new List<Competition> { await _compRepo.GetCompetitionById(id) };
                if (res is not null && res.Count != 0)
                    return Result<List<Competition>>.Success("", res);
                return Result<List<Competition>>.NotFound();
            }
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return Result<List<Competition>>.InternalServerError();
			}
        }

        public async Task<Result<List<Competition>>> GetTopLeaguesAsync()
        {
            try
            {
                var topLeagues = await _compRepo.GetTopLeaguesAsync(LeagueInfo.TopLeagues);
                var result = Result<List<Competition>>.Success("", topLeagues);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                var result = Result<List<Competition>>.InternalServerError("An error occurred while fetching top leagues.");
                return result;
            }
        }
    }
}
