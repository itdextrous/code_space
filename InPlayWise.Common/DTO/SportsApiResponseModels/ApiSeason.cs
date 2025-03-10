using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.SportsApiResponseModels
{
    public class ApiSeason
    {

        [JsonPropertyName("id")]
        public string Id {  get; set; }

        [JsonPropertyName("competition_id")]
        public string CompetitionId { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        [JsonPropertyName("has_player_stats")]
        public int HasPlayerStats { get; set; }

        [JsonPropertyName("has_team_stats")]
        public int HasTeamStats { get; set; }

        [JsonPropertyName("has_table")]
        public int HasTable { get; set; }

        [JsonPropertyName("is_current")]
        public int IsCurrent { get; set; }

        [JsonPropertyName("start_time")]
        public long StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public long EndTime { get; set; }


    }
}
