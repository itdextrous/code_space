using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicInfoResponseModels
{
    public class CategoryResponseDto
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("results")]
        public List<CategoryResponse> Results { get; set; }
    }

    public class CategoryResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("updated_at")]
        public int UpdatedAt { get; set; }
    }
}