using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.Prediction;
using InPlayWise.Data.Entities.PredictionEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;
using InPlayWiseData.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InPlayWise.Data.DbContexts
{
    public class LocalDbContext : IdentityDbContext<ApplicationUser>
	{
		public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }
		public DbSet<EventModel> Events { get; set; }
		public DbSet<ResetPasswordModel> ResetPassword { get; set; }
		public DbSet<MatchStatus> MatchStates { get; set; }
		public DbSet<RecentMatchModel> RecentMatches { get; set; }
		public DbSet<LiveMatchModel> LiveMatches { get; set; }
		public DbSet<PredictionModel> PredictionData { get; set; }
		public DbSet<UserProfile> Profiles { get; set; }
		public DbSet<FavouriteTeams> FavouriteTeams { get; set; }
		public DbSet<FavouriteCompetitions> FavouriteCompetitions { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Price> Prices { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }
		public DbSet<PlanFeatures> PlanFeatures { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Competition> Competitions { get; set; }
		public DbSet<Team> Team { get; set; }
		public DbSet<Season> Seasons { get; set; }
		public DbSet<UpcomingMatch> UpcomingMatches { get; set; }
		public DbSet<MatchAlert> MatchAlerts { get; set; }
		public DbSet<DailyPrediction> DailyPredictionData { get; set; }
		public DbSet<AllPrediction> AllPredictionData { get; set; }
		public DbSet<LiveInsightsPerGame> LiveInsightsCounter { get; set; }
		public DbSet<LeagueStatsCount> LeagueStatsCounter { get; set; }
		public DbSet<PredictionCounter> PredictionCounters { get; set; }
		public DbSet<CleverLabelsCounter> CleverLabelsCounters { get; set; }

		public DbSet<Shocks> Shocks { get; set; }
		public DbSet<ShockCounter> ShockCounter { get; set; }
		public DbSet<UserQuota> UserQuotas { get; set; }
		public DbSet<Advertisement> Advertisements { get; set; }
		public DbSet<LeagueStats> LeagueStats { get; set; }

		public DbSet<DailyPredictionDataset> DailyPredictionDataset { get; set; }
		public DbSet<AllPredictionDataset> AllPredictionDataset { get; set; }

		public DbSet<PredictionRecord> PredictionRecords { get; set; }



		public DbSet<PredictionActiveMatchesData> PredictionActiveMatchesDataset { get; set; }
		public DbSet<PredictionFullData> PredictionFullDataSet { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<FavouriteTeams>()
				.HasOne(ft => ft.User)
				.WithMany(u => u.FavouriteTeams)
				.HasForeignKey(ft => ft.UserId);

			builder.Entity<FavouriteTeams>()
				.HasOne(ft => ft.Teams)
				.WithMany(t => t.FavTeams)
				.HasForeignKey(ft => ft.TeamId);

			builder.Entity<FavouriteCompetitions>()
				.HasOne(fc => fc.User)
				.WithMany(us => us.FavouriteCompetition)
				.HasForeignKey(fc => fc.UserId);

			builder.Entity<FavouriteCompetitions>()
				.HasOne(fc => fc.Competition)
				.WithMany(comp => comp.favComps)
				.HasForeignKey(fc => fc.CompetitionId);

			builder.Entity<Subscription>()
				.HasOne(sub => sub.Product)
				.WithMany(prod => prod.Subscriptions)
				.HasForeignKey(sub => sub.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<PlanFeatures>()
				.HasOne(ft => ft.Product)
				.WithOne(prod => prod.Features)
				.HasForeignKey<PlanFeatures>(ft => ft.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Product>()
				.HasMany(prod => prod.Price)
				.WithOne(price => price.Product)
				.HasForeignKey(price => price.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Team>()
				.HasIndex(tm => tm.Name);

			builder.Entity<UpcomingMatch>()
				.HasOne(m => m.HomeTeam)
				.WithMany(t => t.UpcomingHomeMatches)
				.HasForeignKey(m => m.HomeTeamId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<UpcomingMatch>()
				.HasOne(m => m.AwayTeam)
				.WithMany(t => t.UpcomingAwayMatches)
				.HasForeignKey(m => m.AwayTeamId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<MatchAlert>()
				.HasOne(ma => ma.UpcomingMatch)
				.WithMany(um => um.MatchAlerts)
				.HasForeignKey(ma => ma.MatchId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<MatchAlert>()
				.HasOne(ma => ma.User)
				.WithMany(us => us.MatchAlerts)
				.HasForeignKey(ma => ma.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<UserProfile>()
				.HasOne(up => up.User)
				.WithOne(us => us.Profile)
				.HasForeignKey<UserProfile>(u => u.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<FeatureCounter>()
				.HasOne(fc => fc.UserProfile)
				.WithOne(up => up.Features)
				.HasForeignKey<FeatureCounter>(fc => fc.UserId)
				.OnDelete(DeleteBehavior.Cascade);




			//builder.Entity<FeatureCounter>()
			//	.HasMany(fc => fc.LiveInsightsPerGame)
			//	.WithOne(lg => lg.FeatureCounter)
			//	.HasForeignKey(lg => lg.FeatureCounterId)
			//	.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<LiveInsightsPerGame>()
				.HasOne(lg => lg.Match)
				.WithMany(mc => mc.LiveInsightsCounter)
				.HasForeignKey(lg => lg.MatchId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<UpcomingMatch>()
				.HasOne(um => um.Competition)
				.WithMany(cmp => cmp.UpcomingMatches)
				.HasForeignKey(um => um.CompetitionId)
				.OnDelete(DeleteBehavior.Cascade);






			// These are for feature counter services

			builder.Entity<LiveInsightsPerGame>()
				.HasOne(li => li.Match)
				.WithMany(match => match.LiveInsightsCounter)
				.HasForeignKey(li => li.MatchId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<LiveInsightsPerGame>()
				.HasOne(li => li.User)
				.WithMany(us => us.LiveInsightsPerGamesCounter)
				.HasForeignKey(li => li.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<LeagueStatsCount>()
				.HasOne(lsc => lsc.User)
				.WithMany(us => us.LeagueStatsCounter)
				.HasForeignKey(ls => ls.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<LeagueStatsCount>()
				.HasOne(lsc => lsc.Competition)
				.WithMany(cmp => cmp.LeagueStatsCounter)
				.HasForeignKey(ls => ls.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<PredictionCounter>()
				.HasOne(pc => pc.Match)
				.WithMany(match => match.PredictionCounter)
				.HasForeignKey(pc => pc.MatchId)
				.OnDelete(DeleteBehavior.Cascade);


			builder.Entity<PredictionCounter>()
				.HasOne(pc => pc.User)
				.WithMany(match => match.PredictionCounter)
				.HasForeignKey(pc => pc.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<CleverLabelsCounter>()
				.HasOne(clc => clc.Team)
				.WithMany(team => team.CleverLabelsCounter)
				.HasForeignKey(clc => clc.TeamId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<CleverLabelsCounter>()
				.HasOne(clc => clc.User)
				.WithMany(us => us.CleverLabelsCounter)
				.HasForeignKey(clc => clc.UserId)
				.OnDelete(DeleteBehavior.Cascade);



			// Counter relations end here



			builder.Entity<Shocks>()
				.HasOne(sh => sh.Team)
				.WithMany(tm => tm.Shocks)
				.HasForeignKey(sh => sh.TeamId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Shocks>()
				.HasOne(sh => sh.Match)
				.WithMany(m => m.Shocks)
				.HasForeignKey(sh => sh.MatchId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<ShockCounter>()
				.HasOne(sh => sh.Match)
				.WithMany(match => match.ShockCounter)
				.HasForeignKey(sh => sh.Matchid)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<ShockCounter>()
				.HasOne(sh => sh.User)
				.WithOne(us => us.ShockCounter)
				.HasForeignKey<ShockCounter>(sh => sh.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<UserQuota>()
				.HasOne(uq => uq.User)
				.WithOne(us => us.UserQuota)
				.HasForeignKey<UserQuota>(uq => uq.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<LeagueStats>()
				.HasOne(ls => ls.Competition)
				.WithOne(comp => comp.LeagueStats)
				.HasForeignKey<LeagueStats>(ls => ls.CompetitionId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<LeagueStats>()
				.Property(ls => ls.SeasonId)
				.IsRequired();

			builder.Entity<PredictionActiveMatchesData>()
				.HasOne(pm => pm.match)
				.WithMany(lm => lm.PredictionActiveMatches)
				.HasForeignKey(pm => pm.MatchId)
				.OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RecentMatchModel>()
    .HasOne(rm => rm.HomeTeam)
    .WithMany(ht => ht.HomeRecentMatches)
    .HasForeignKey(rm => rm.HomeTeamId)
    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RecentMatchModel>()
                .HasOne(rm => rm.AwayTeam)
                .WithMany(at => at.AwayRecentMatches)
                .HasForeignKey(rm => rm.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RecentMatchModel>()
                .HasOne(rm => rm.Competition)
                .WithMany(comp => comp.RecentMatches)
                .HasForeignKey(rm => rm.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);

        }

	}
}
