
using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels
{

    public class Query
    {
        [JsonPropertyName("total")]
        public int? Total { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("page")]
        public int? Page { get; set; }
    }

    public class Coverage
    {
        [JsonPropertyName("lineup")]
        public int Lineup { get; set; }

        [JsonPropertyName("mlive")]
        public int Mlive { get; set; }

    }


    public class Environment
    {
        [JsonPropertyName("humidity")]
        public string Humidity { get; set; }

        [JsonPropertyName("pressure")]
        public string Pressure { get; set; }

        [JsonPropertyName("temperature")]
        public string Temperature { get; set;}

        [JsonPropertyName("weather")]
        public int Weather { get; set; }

        [JsonPropertyName("wind")]
        public string Wind { get; set; }

    }

    public class Match
    {
        [JsonPropertyName("away_position")]
        public string AwayPosition { get; set; }

        [JsonPropertyName("away_scores")]
        public List<int> Away_Scores { get; set; }

        [JsonPropertyName("away_team_id")]
        public string AwayTeamId { get; set; }

        [JsonPropertyName("competition_id")]
        public string CompetitionId { get; set; }

        [JsonPropertyName("coverage")]
        public Coverage Coverage { get; set; }

        [JsonPropertyName("environment")]
        public Environment Environment { get; set; }

        [JsonPropertyName("home_position")]
        public string HomePosition { get; set; }

        [JsonPropertyName("home_scores")]
        public List<int> HomeScores { get; set; }

        [JsonPropertyName("home_team_id")]
        public string HomeTeamId { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("match_time")]
        public long MatchTime { get; set; }

        [JsonPropertyName("neutral")]
        public int Neutral {get; set;}

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("referee_id")]
        public string RefereeId { get; set; }

        [JsonPropertyName("round")]
        public Round Round { get; set; }

        [JsonPropertyName("season_id")]
        public string SeasonId { get; set; }

        [JsonPropertyName("status_id")]
        public int StatusId { get; set; }
        [JsonPropertyName("updated_at")]
        public long UpdatedAt { get; set; }

        [JsonPropertyName("venue_id")]
        public string VenueId { get; set; }



        //public List<HomeScores> HomeScores { get; set; }


        //public string AwayPosition { get; set; }


        //public string RelatedId { get; set; }
        //public List<int> AggScore { get; set; }

    }


    public class MatchRecentDto
    {
        [JsonPropertyName("code")]
        public int? Code { get; set; }
        [JsonPropertyName("query")]
        public Query Query { get; set; }

        [JsonPropertyName("results")]
        public List<Match> Results { get; set; }
    }
}
