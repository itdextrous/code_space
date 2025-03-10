using InPlayWise.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace InPlayWise.Common.DTO.CachedDto
{
    public class OpportunityCachedDto
    {
        public new Guid Id { get; set; }
        public string UserId { get; set; }
        public string OpportunityName { get; set; }
        public float ProbabilityPercentage { get; set; }
        public string MatchId { get; set; }
        public string GroupId { get; set; }
        public DateTime SavedTime { get; set; }
        public float Odds { get; set; }

        public bool IsHedged { get; set; } = false;

        public ConfidencyLevel ConfidencyLevel { get; set; }

        public int OppPlaced { get; set; }
        public OpportunityResult OppResult { get; set; }
        public int ResultTime { get; set; }

        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }

        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }

        public int MatchEndMinutes { get; set; }

        [NotMapped]
        public int MatchStatus { get; set; }
        [NotMapped]
        public double KickoffTimeStamp { get; set; }
    }
}
