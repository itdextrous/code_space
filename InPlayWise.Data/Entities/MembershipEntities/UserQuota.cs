using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities.MembershipEntities
{
	public class UserQuota
	{
		[Key]
		public string UserId { get; set; }
		public int TotalPrediction { get; set; }
		public int AccumulatorGenerators { get; set; }
		public int ShockDetectors { get; set; }
		public int CleverLabels { get; set; }
		public int HistoryOfAccumulators { get; set; }
		public int WiseProHedge { get; set; }
		public int LeagueStatistics { get; set; }
		public int LiveInsightsPerGame { get; set; }
		public bool WiseProIncluded { get; set; }
		public ApplicationUser User { get; set; }
	}
}
