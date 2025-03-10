using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels
{
	public class DataList
	{
		[JsonPropertyName("id")]
		public string MatchId { get; set; }
		public int Status { get; set; }
		public List<int> HomeScore { get; set; }
		public List<int> AwayScore { get; set; }
		public long KickOffTimeStamp { get; set; }

		[JsonPropertyName("stats")]
		public List<Stat> Stats { get; set; }

		[JsonPropertyName("incidents")]
		public List<Incident> Incidents { get; set; }

	}
}
