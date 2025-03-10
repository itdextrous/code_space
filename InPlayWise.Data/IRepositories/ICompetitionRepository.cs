using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.IRepositories
{
	public interface ICompetitionRepository
	{



		Task<Competition> GetCompetitionById(string compId);

		Task<bool> UpdateCompetition(Competition comp);


		Task<List<Competition>> GetByName(string name);
		Task<List<Competition>> GetByCountry(string country);
		Task<List<Competition>> GetFiftyCompetition();

		Task<List<Competition>> GetAllCompetition();

		Task<bool> AddCategory(CompetionCategory cc);

		Task<List<CompetionCategory>> GetAllCategories();

		Task<List<Competition>> GetAllLeagues();

        Task<List<Competition>> GetTopLeaguesAsync(List<string> topLeagues);



    }
}
