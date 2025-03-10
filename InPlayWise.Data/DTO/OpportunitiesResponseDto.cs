using InPlayWise.Data.Entities;
using System.Text.Json.Serialization;

namespace InPlayWise.Data.DTO
{
    public class OpportunitiesResponseDto
    {
        public string MatchId { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeLogo { get; set; }
        public string AwayLogo { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionLogo { get; set; }
        public long CurrentKickoffTime { get; set; }

        public int MatchStatus { get; set; }

        [JsonPropertyName("opportunities")]
        public List<Opportunity> Opportunities { get; set; } = new List<Opportunity>();

    }
}
