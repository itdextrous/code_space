using InPlayWise.Core.BackgroundProcess.Interface;
using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Identity;

namespace InPlayWiseApi.Extensions
{
    public static class MiddlewareExtension
    {
        public static async Task SeedData(this WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "admin", "user"};
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }


            //ISportsApiDataSeedService sportsApiDataSeed = serviceScope.ServiceProvider.GetRequiredService<ISportsApiDataSeedService>();
            //await sportsApiDataSeed.SeedCategory();
            //await sportsApiDataSeed.SeedCountry();
            //await sportsApiDataSeed.SeedCompetitions();
            //await sportsApiDataSeed.SeedTeams();
            //await sportsApiDataSeed.SeedUpcomingMatches();
            //await sportsApiDataSeed.SeedSeason();


            var recentMatch = serviceScope.ServiceProvider.GetRequiredService<IHistoricMatchBackgroundProcess>();
            var teamsBackgroundService = serviceScope.ServiceProvider.GetRequiredService<ITeamsBackgroundProcess>();
            await teamsBackgroundService.SeedCompetitionsInMemory();
            await teamsBackgroundService.SeedTeamsInMemory();
            await recentMatch.SeedMatchesInMemory();





            // Below are websocket endpoints


        }
    }
}
