using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels
{
	public class Stat
	{

		[JsonPropertyName("type")]
		public int Type { get; set; }


		[JsonPropertyName("home")]
		public int Home { get; set; }


		[JsonPropertyName("away")]
		public int Away { get ; set; }

	}
}
