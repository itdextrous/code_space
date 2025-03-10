using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicInfoResponseModels
{
    public class StageResponseModel
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        //[JsonPropertyName("query")]
        //public Inquiry Query { get; set; }

        [JsonPropertyName("results")]
        public List<Stage> Results { get; set; }
    }

    public class Stage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("season_id")]
        public string SeasonId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("mode")]
        public int Mode { get; set; }

        [JsonPropertyName("group_count")]
        public int GroupCount { get; set; }

        [JsonPropertyName("round_count")]
        public int RoundCount { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("updated_at")]
        public int UpdatedAt { get; set; }
    }
}
