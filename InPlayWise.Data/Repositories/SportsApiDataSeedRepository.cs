using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;

namespace InPlayWise.Data.Repositories
{
    public class SportsApiDataSeedRepository : ISportsApiDataSeedRepository
	{
		private readonly AppDbContext _db;
		public SportsApiDataSeedRepository(AppDbContext db)
		{
			_db = db;
            _db.Database.SetCommandTimeout(3000);
        }
		public async Task<bool> AddCategories(List<Category> categories)
		{
			try
			{
				await _db.Categories.AddRangeAsync(categories);
				await _db.SaveChangesAsync();
				return true;
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> AddCompetition(List<Competition> competitions)
		{
			try
			{
				await _db.Competitions.AddRangeAsync(competitions);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> AddCountries(List<Country> countries)
		{
			try
			{
				await _db.Countries.AddRangeAsync(countries);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> AddSeasons(List<Season> seasons)
		{
			try
			{
				await _db.Seasons.AddRangeAsync(seasons);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

        public Task<bool> AddTeam(Team team)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddTeams(List<Team> teams)
		{
			try
			{
				await _db.Team.AddRangeAsync(teams);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<bool> AddUpcomingMatches(List<UpcomingMatch> matches)
		{
			try
			{
				foreach(var match in matches)
				{
					try
					{
						_db.UpcomingMatches.Add(match);
						await _db.SaveChangesAsync();
					}
					catch(Exception ex)
					{
						Console.WriteLine(ex);
						return true;
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

        public async Task<bool> TeamExists(string teamId)
        {
            try
            {
                return (await _db.Team.FindAsync(teamId)) is not null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> AddOrUpdateTeam(Team team)
        {
            try
            {
				Team tm = await _db.Team.FindAsync(team.Id);
				if(tm is null)
				{
					await _db.Team.AddAsync(team);
					await _db.SaveChangesAsync();
					return true;
				}
                _db.Entry(tm).CurrentValues.SetValues(team);
				await _db.SaveChangesAsync();
				return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> AddOrUpdateCompetition(Competition comp)
        {
            try
            {
                Competition cm = await _db.Competitions.FindAsync(comp.Id);
                if (cm is null)
                {
                    await _db.Competitions.AddAsync(comp);
                    await _db.SaveChangesAsync();
                    return true;
                }
                _db.Entry(cm).CurrentValues.SetValues(comp);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

		public async Task<List<string>> GetAllTeamsId()
		{
			try
			{
				return await _db.Team.OrderBy(team => team.Id).Select(team => team.Id).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<bool> SaveTeamsList(List<Team> teams)
		{
			try
			{
				await _db.AddRangeAsync(teams);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<List<string>> GetAllCompetitionId()
		{
			try
			{
				return await _db.Competitions.OrderBy(comp => comp.Id).Select(comp => comp.Id).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public async Task<bool> SaveCompetitionList(List<Competition> competitions)
		{
			try
			{
				await _db.AddRangeAsync(competitions);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public Task<List<UpcomingMatch>> GetAllUpcomingMatches()
		{
			try
			{
				return _db.UpcomingMatches.Include(um => um.HomeTeam).Include(um => um.AwayTeam).Include(um => um.Competition).OrderBy(um => um.time).ToListAsync();
			}catch(Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public Task<List<string>> GetIdOfExistingUpcomingMatches()
		{
			try
			{
				return _db.UpcomingMatches.Select(um => um.Id).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

        public async Task<bool> DeleteAllUpcomingMatches()
        {
			try
			{
				List<UpcomingMatch> matches = await _db.UpcomingMatches.ToListAsync();
				_db.UpcomingMatches.RemoveRange(matches);
				await _db.SaveChangesAsync();
				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return false; 
			}
        }

        public async Task<List<Competition>> GetAllCompetitions()
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

        public async Task<bool> UpdateTeamCount(List<Competition> comps)
        {
            try
            {
                List<Competition> dbComps = await _db.Competitions.Where(cm => cm.Type == 1).ToListAsync();
				foreach (Competition comp in dbComps)
				{
					int count = (dbComps.FirstOrDefault(c => c.Id.Equals(comp.Id))).TeamCount;
					comp.TeamCount = count;
				}
				await _db.SaveChangesAsync();
				return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<string>> GetTeamsWithIncompleteInfo()
        {
			try
			{
				return await _db.Team.Where(team => (string.IsNullOrEmpty(team.Id)
						|| string.IsNullOrEmpty(team.Name) ))
					.Select(team => team.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<string>> GetAllCategoriesId()
        {
            try
            {
				return await _db.Categories.Select(cat => cat.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<string>> GetAllContriesId()
        {
            try
            {
                return await _db.Countries.Select(cat => cat.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<string>> GetCompetitionsWithIncompleteInfo()
        {
            try
            {
				return await _db.Competitions.Where(comp => string.IsNullOrEmpty(comp.Id)
					|| string.IsNullOrEmpty(comp.Name) || string.IsNullOrEmpty(comp.Logo) || comp.Type == 0).Select(comp => comp.Id)
					.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> UpdateMultipleTeams(List<Team> teams)
        {
            try
            {
				List<string> teamIds = teams.Select(team => team.Id).ToList();
				//List<Team> dbTeams = await _db.Team.Where()
				List<Team> dbTeams = await _db.Team.Where(tm => teamIds.Contains(tm.Id)).ToListAsync();
				foreach (Team team in dbTeams) {
                    var updatedTeam = teams.SingleOrDefault(tm => tm.Id == team.Id);
                    if (updatedTeam != null)
                    {
                        _db.Entry(team).CurrentValues.SetValues(updatedTeam);
                    }
                }
				await _db.SaveChangesAsync();
                //Team tm = await _db.Team.FindAsync(team.Id);
                //if (tm is null)
                //{
                //    await _db.Team.AddAsync(team);
                //    await _db.SaveChangesAsync();
                //    return true;
                //}
                //_db.Entry(tm).CurrentValues.SetValues(team);
                //await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateMultipleCompeitions(List<Competition> competitions)
        {
			try { 
            List<string> competitionIds = competitions.Select(comp => comp.Id).ToList();
            //List<Team> dbTeams = await _db.Team.Where()
            List<Competition> dbComps = await _db.Competitions.Where(cm => competitionIds.Contains(cm.Id)).ToListAsync();
            foreach (Competition comp in dbComps)
            {
                var updatedComp = competitions.SingleOrDefault(cm => cm.Id == comp.Id);
                if (updatedComp != null)
                {
                    _db.Entry(comp).CurrentValues.SetValues(updatedComp);
                }
            }
            await _db.SaveChangesAsync();
            //Team tm = await _db.Team.FindAsync(team.Id);
            //if (tm is null)
            //{
            //    await _db.Team.AddAsync(team);
            //    await _db.SaveChangesAsync();
            //    return true;
            //}
            //_db.Entry(tm).CurrentValues.SetValues(team);
            //await _db.SaveChangesAsync();
            return true;
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
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

        public async Task<List<Competition>> GetAllLeaguesWithoutTeamCount()
        {
            try
            {
                return await _db.Competitions.Where(c => c.Type == 1 && c.TeamCount < 1).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
