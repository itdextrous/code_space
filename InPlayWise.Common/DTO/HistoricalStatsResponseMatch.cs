namespace InPlayWise.Common.DTO
{
    public class HistoricalStatsResponseMatch
    {
        public string MatchId { get ; set; }
        public HistoricalStatsResponseTeam HomeTeamStats { get; set; } 
        public HistoricalStatsResponseTeam AwayTeamStats { get; set; }
    }
}
