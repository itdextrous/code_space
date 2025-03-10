using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.SportsApiResponseModels
{
	public class ApiCountry
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("category_id")]
		public string CategoryId { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("logo")]
		public string Logo { get; set; }

		[JsonPropertyName("updated_at")]
		public long UpdatedAt { get; set; }


	}
}
