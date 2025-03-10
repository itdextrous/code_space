using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicInfoResponseModels
{
    public class VenueResponseModel
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        //[JsonPropertyName("query")]
        //public Inquiry Query { get; set; }

        [JsonPropertyName("results")]
        public List<Venue> Results { get; set; }
    }

    public class Venue
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("country_id")]
        public string CountryId { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("updated_at")]
        public int UpdatedAt { get; set; }
    }
}
