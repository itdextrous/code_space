using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicInfoResponseModels
{
    public class DataUpdateResponseModel
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("results")]
        public Results Results { get; set; }
    }

    public class DataTypeId
    {
        [JsonPropertyName("match_id")]
        public string MatchId { get; set; }

        [JsonPropertyName("season_id")]
        public string SeasonId { get; set; }

        [JsonPropertyName("pub_time")]
        public int PubTime { get; set; }

        [JsonPropertyName("update_time")]
        public int UpdateTime { get; set; }
    }

    public class Results
    {
        [JsonPropertyName("type_id")]
        public Dictionary<string, DataTypeId> TypeId { get; set; }
    }
}