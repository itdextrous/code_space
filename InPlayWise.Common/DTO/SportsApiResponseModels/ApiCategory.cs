using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.SportsApiResponseModels
{
	public class ApiCategory
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("updated_at")]
		public long UpdatedAt { get; set; }
	}
}
