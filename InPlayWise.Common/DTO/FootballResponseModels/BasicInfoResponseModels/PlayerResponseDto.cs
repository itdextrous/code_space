//using System.Collections.Generic;
//using System.Text.Json.Serialization;

//namespace InPlayWise.Common.DTO.FootballResponseModels.BasicInfoResponseModels
//{
//    public class PlayerResponseDto
//    {
//        [JsonPropertyName("code")]
//        public int code { get; set; }

//        [JsonPropertyName("query")]
//        public Inquiry query { get; set; }

//        [JsonPropertyName("results")]
//        public List<Player> results { get; set; }
//    }

//    public class AbilityScore
//    {
//        [JsonPropertyName("type_id")]
//        public int TypeId { get; set; }

//        [JsonPropertyName("rating")]
//        public int Rating { get; set; }

//        [JsonPropertyName("average_score")]
//        public int AverageScore { get; set; }
//    }

//    public class TechnicalFeature
//    {
//        [JsonPropertyName("advantages")]
//        public List<List<int>> Advantages { get; set; }

//        [JsonPropertyName("disadvantages")]
//        public List<List<int>> Disadvantages { get; set; }
//    }

//    public class Position
//    {
//        [JsonPropertyName("main_location")]
//        public string MainLocation { get; set; }

//        [JsonPropertyName("secondar_position")]
//        public List<string> SecondaryPosition { get; set; }
//    }

//    public class Player
//    {
//        [JsonPropertyName("id")]
//        public string id { get; set; }

//        [JsonPropertyName("team_id")]
//        public string team_id { get; set; }

//        [JsonPropertyName("name")]
//        public string name { get; set; }

//        [JsonPropertyName("short_name")]
//        public string short_name { get; set; }

//        [JsonPropertyName("logo")]
//        public string logo { get; set; }

//        [JsonPropertyName("national_logo")]
//        public string national_logo { get; set; }

//        [JsonPropertyName("age")]
//        public int age { get; set; }

//        [JsonPropertyName("birthday")]
//        public int birthday { get; set; }

//        [JsonPropertyName("weight")]
//        public int weight { get; set; }

//        [JsonPropertyName("height")]
//        public int height { get; set; }

//        [JsonPropertyName("country_id")]
//        public string country_id { get; set; }

//        [JsonPropertyName("nationality")]
//        public string nationality { get; set; }

//        [JsonPropertyName("market_value")]
//        public int market_value { get; set; }

//        [JsonPropertyName("market_value_currency")]
//        public string market_value_currency { get; set; }

//        [JsonPropertyName("contract_until")]
//        public int contract_until { get; set; }

//        [JsonPropertyName("preferred_foot")]
//        public int preferred_foot { get; set; }

//        [JsonPropertyName("ability")]
//        [JsonIgnore]
//        public List<AbilityScore> ability { get; set; }

//        [JsonIgnore]
//        [JsonPropertyName("characteristics")]
//        public List<TechnicalFeature> characteristics { get; set; }

//        [JsonPropertyName("position")]
//        public string position { get; set; }

//        [JsonPropertyName("positions")]
//        [JsonIgnore]
//        public List<Position> positions { get; set; }

//        [JsonPropertyName("updated_at")]
//        public int updated_at { get; set; }
//    }
//}
