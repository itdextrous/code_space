using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels
{

    public class RealTimeMatchStructure
    {
        public RealTimeDataList RealTimeDataList { get; set; }
    }

    public class RealTimeDataList
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("score")]
        public List<Score> Score { get; set; }

        [JsonIgnore]
        [JsonPropertyName("incidents")]
        public List<Object> Incidents { get; set; }
    }

    public class Score {
        short Type { get; set; }
        short Home { get; set; }
        short Away { get; set; }
    }



    public class RealTimeDataResponseDto
    {

        [JsonPropertyName("code")]
        public int Code { get; set; }


        [JsonPropertyName("results")]
        public List<RealTimeDataList> Results { get; set; }

    }
}
