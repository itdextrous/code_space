using InPlayWise.Common.Enums;

namespace InPlayWise.Common.DTO
{
    public class OpportunityDto
    {
        public string OpportunityName { get; set; }
        public float ProbabilityPercentage { get; set; }
        public string MatchId { get; set; }
        public float Odds { get; set; }
        public bool IsHedged { get; set; } = false;
        public ConfidencyLevel ConfidencyLevel { get; set; } 
    }
}
