using InPlayWise.Data.Entities.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
	public interface ICompetitionService
	{
		Task<Result<List<Competition>>> GetCompetitionByName(string name);

		Task<Result<List<Competition>>> GetCompetitionByCountry(string name);
		Task<Result<List<Competition>>> GetFiftyCompetiton();
		Task<bool> AddCategory();
		Task<Result<List<Competition>>> GetCompetition(string id, bool isLeague);

        Task<Result<List<Competition>>> GetTopLeaguesAsync();
        //Task<List<Competition>> GetAllCompetition();
    }
}
