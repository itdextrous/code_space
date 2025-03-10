namespace InPlayWise.Common.DTO
{
	public class FeaturesDto
	{
		public string ProductId { get; set; }
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

	}
}
