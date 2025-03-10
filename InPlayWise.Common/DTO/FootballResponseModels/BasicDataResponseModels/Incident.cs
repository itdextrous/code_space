using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels
{
	public class Incident
	{
		[JsonPropertyName("type")]
		public int Type { get; set; }

		[JsonPropertyName("position")]
		public int Position { get; set; }

		[JsonPropertyName("time")]
		public int Time { get; set; }

		[JsonPropertyName("player_name")]
		public string PlayerName { get; set; }

		[JsonPropertyName("in_player_name")]
		public string InSubPlayer { get; set; }
	}
}
