using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.SportsEntities;
using System.Text.Json.Serialization;

namespace InPlayWise.Data.Entities.SportsEntities
{
	public class Competition
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string ShortName { get; set; }
		public string Logo { get; set; }
		public string CurrentSeasonId { get; set; }
		public string CurrentStageId { get; set; }
		public int CurrentRound { get; set; }
		public int TotalRounds { get; set; }
		public string CountryId { get; set; }
		public string PrimaryColor { get; set; }
		public string SecondaryColor { get; set; }
		public int LeagueLevel { get; set; }
		public int TeamCount { get; set; }
        public List<CompetionCategory> CompetitionCategories { get; set; }
		public int Type { get; set; }


        [JsonIgnore]
		public List<FavouriteCompetitions> favComps { get; set; }

		[JsonIgnore]
		public List<UpcomingMatch> UpcomingMatches { get; set; }

		[JsonIgnore]
		public LeagueStats LeagueStats { get; set; }

		[JsonIgnore]
		public List<LeagueStatsCount> LeagueStatsCounter { get; set; }
		public List<LiveMatchModel> LiveMatches { get; set; }
		public List<RecentMatchModel> RecentMatches { get; set; }
	}
}
