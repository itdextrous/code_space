using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.SportsEntities;
using System.Text.Json.Serialization;

namespace InPlayWise.Data.Entities.SportsEntities
{
    public class Team
	{
		public string Id { get; set; }
		public string CompetitionId { get; set; }
		public string CountryId { get; set; }
		public string Name { get; set; }
		public string ShortName { get; set; }
		public string Logo { get; set; }
		public bool IsNational { get; set; }
		//CountryLogo exists for national teams only
		public string CountryLogo { get; set; }
		public long FoundationTime { get; set; }

		public List<LiveMatchModel> HomeLiveMatches { get; set; }
		public List<LiveMatchModel> AwayLiveMatches { get; set; }

		public List<RecentMatchModel> HomeRecentMatches { get; set;}
		public List<RecentMatchModel> AwayRecentMatches { get; set;}

		[JsonIgnore]
		public List<FavouriteTeams> FavTeams { get; set; }

		[JsonIgnore]
		public List<UpcomingMatch> UpcomingHomeMatches { get; set; }

		[JsonIgnore]
		public List<UpcomingMatch> UpcomingAwayMatches { get; set; }

		[JsonIgnore]
		public List<Shocks> Shocks { get; set; }

		[JsonIgnore]
		public List<CleverLabelsCounter> CleverLabelsCounter { get; set; }
	}
}