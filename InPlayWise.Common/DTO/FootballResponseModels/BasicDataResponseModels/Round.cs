using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels
{
	public class Round
	{
		[JsonPropertyName("group_num")]
		public int GroupNum { get; set; }

		[JsonPropertyName("round_num")]
		public int RoundNum { get; set; }

		[JsonPropertyName("stage_id")]
		public string StageId { get; set; }
	}
}
