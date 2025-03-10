using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;


namespace InPlayWise.Data.Repositories
{
    public class TeamsRepository : ITeamsRepository
    {

        private readonly AppDbContext _db;

        public TeamsRepository(AppDbContext db) {
            _db = db;
        }

        public async Task<List<Competition>> GetAllCompetions()
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

        public async Task<List<Team>> GetAllTeams()
        {
            try
            {
                return await _db.Team.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Team>> GetFiftyTeams()
        {
            try
            {
                return await _db.Team.Where(tm => tm.IsNational).Take(50).ToListAsync();       
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<Team> GetTeamById(string teamId)
        {
            try
            {
                return await _db.Team.FindAsync(teamId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Team>> GetTeamsByCompetitionName(string competitionName)
        {
            try
            {
                List<Competition> comps =  await _db.Competitions.Where(cm => cm.Name.Contains(competitionName)).Take(10).ToListAsync();
                List<Team> teams = new List<Team>();
                foreach(Competition comp in comps)
                {
                    List<Team> tm = await _db.Team.Where(tm => tm.CompetitionId.Equals(comp.Id)).ToListAsync();
                    teams.AddRange(tm);
                }
                return teams;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<Team>> GetTeamsByCountryName(string countryName)
        {
            try
            {
                List<Country> countries = await _db.Countries.Where(cn => cn.Name.Contains(countryName)).ToListAsync();
                List<Team> teams = new List<Team>();
                foreach(Country country in countries)
                {
                    List<Team> tm = await _db.Team.Where(tm => tm.CountryId.Equals(country.Id)).ToListAsync();
                    teams.AddRange(tm);
                }
                return teams;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<List<Team>> GetTeamsByName(string teamName)
        {
            try
            {
                return await _db.Team.Where(tm => tm.Name.Contains(teamName)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
