namespace InPlayWise.Data.Entities.SportsEntities
{
	public class Season
	{
		public string Id { get; set; }
		public string CompetitionId { get; set; }
		public string Year { get; set; }
		public bool HasPlayerStats { get; set; }
		public bool HasTeamStats { get; set; }
		public bool IsCurrent { get; set; }
		public long StartTime { get; set; }
		public long EndTime { get; set; }

	}
}
