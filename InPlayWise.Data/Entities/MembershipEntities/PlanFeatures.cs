namespace InPlayWise.Data.Entities.MembershipEntities
{
	public class PlanFeatures
	{
		public Guid Id { get; set; }
		public int LiveInsightPerGame { get; set; }
		public int LivePredictionPerGAme { get; set; }
		public int MaxPredictions { get; set; }
		public int AccumulatorGenerators { get; set; }
		public int ShockDetectors { get; set; }
		public int CleverLabelling { get; set; }
		public int HistoryOfAccumulators { get; set; }
		public int WiseProHedge { get; set; }
		public int LeagueStatistics { get; set; }
		public bool WiseProIncluded { get; set; }
		public Guid ProductId { get; set; }
		public Product Product { get; set; }

	}
}
