using InPlayWise.Data.Entities;

namespace InPlayWise.Data.DTO
{
    public class MatchInsightsResponseDto
    {

        public MatchInsightsResponseDto()
        {
            HomeInsights = new Insights();
            AwayInsights = new Insights();
        }

        public Insights HomeInsights { get; set; }
        public Insights AwayInsights { get; set; }
    }
}
