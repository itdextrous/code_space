using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace InPlayWise.Data.Repositories
{
	public class CompetitionRepository : ICompetitionRepository
	{

		private readonly AppDbContext _db;
		public CompetitionRepository(AppDbContext db) {
			_db = db;
		}

        public async Task<bool> AddCategory(CompetionCategory cc)
        {
            try
            {
				await _db.CompetionCategories.AddAsync(cc);
				await _db.SaveChangesAsync();
				return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<CompetionCategory>> GetAllCategories()
        {
            try
            {
                return await _db.CompetionCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Competition>> GetAllCompetition()
        {
            try
            {
                return await _db.Competitions.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Competition>> GetAllLeagues()
        {
            try
            {
                return await _db.Competitions.Where(comp => comp.LeagueLevel != -1).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Competition>> GetByCountry(string country)
		{
			try
			{
				return await _db.Competitions.Where(com => com.CountryId.Contains(country)).ToListAsync();
			}catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<List<Competition>> GetByName(string name)
		{
			try
			{
				return await _db.Competitions.Where(com => com.Name.Contains(name)).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

        public async Task<Competition> GetCompetitionById(string compId)
        {
            try
            {
                return (await _db.Competitions.Where(comp => comp.Id.Equals(compId)).ToListAsync())[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Competition>> GetFiftyCompetition()
		{
			try
			{
				return await _db.Competitions.Include(comp => comp.CompetitionCategories).Take(50).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

        public async Task<List<Competition>> GetTopLeaguesAsync(List<string> topLeagues)
        {
            try
            {
                var leagueNames = await _db.Competitions
                                 .Where(c => topLeagues.Contains(c.Id))
                                 .ToListAsync();
                return leagueNames;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Competition>();
            }
        }

        public async Task<bool> UpdateCompetition(Competition comp)
        {
            try
            {
                var exComp = await _db.Competitions.FindAsync(comp.Id);
                if (exComp is null)
                    return false;
                _db.Entry(exComp).CurrentValues.SetValues(comp);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
