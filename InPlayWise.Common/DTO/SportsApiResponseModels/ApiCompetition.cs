using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.SportsApiResponseModels
{
    public class ApiCompetition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("category_id")]
        public string CategoryId { get; set; }

        [JsonPropertyName("country_id")]
        public string CountryId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("logo")]
        public string Logo { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("cur_season_id")]
        public string CurrentSeasonId { get; set; }

        [JsonPropertyName("cur_stage_id")]
        public string CurrentStageId { get; set; }

        [JsonPropertyName("cur_round")]
        public int CurrentRound { get; set; }

        [JsonPropertyName("round_count")]
        public int RoundCount { get; set; }

        [JsonPropertyName("primary_color")]
        public string PrimaryColor { get; set; }

        [JsonPropertyName("secondary_color")]
        public string SecondaryColor { get; set; }

    }
}
