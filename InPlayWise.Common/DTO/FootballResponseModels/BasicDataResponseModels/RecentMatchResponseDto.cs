using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels
{
	public class RecentMatchResponseDto
	{

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("competition_id")]
		public string CompetitionId { get ; set; }

		[JsonPropertyName("season_id")]
		public string SeasonId { get; set; }

		[JsonPropertyName("home_team_id")]
		public string HomeTeamId { get;set; }

		[JsonPropertyName("away_team_id")]
		public string AwayTeamId { get; set; }

		[JsonPropertyName("status_id")]
		public int StatusId { get; set; }

		[JsonPropertyName("match_time")]
		public long MatchTime { get; set; }

		[JsonPropertyName("venue_id")]
		public string VenueId { get; set; }

		[JsonPropertyName("referee_id")]
		public string RefereeId { get; set; }

		[JsonPropertyName("note")]
		public string Note { get; set; }

		[JsonPropertyName("home_scores")]
		public List<int> HomeScores { get; set; }

		[JsonPropertyName("away_scores")]
		public List<int> AwayScores { get; set; }

		[JsonPropertyName("home_position")]
		public string HomePosition { get; set; }

		[JsonPropertyName("away_position")]
		public string AwayPosition { get; set; }

		[JsonPropertyName("round")]
		public Round Round { get; set; }

		[JsonPropertyName("related_id")]
		public string RelatedId { get; set; }

		[JsonPropertyName("agg_score")]
		public List<int> AggScore { get; set; }

		[JsonPropertyName("updated_at")]
		public long UpdatedAt { get; set; }

	}
}
