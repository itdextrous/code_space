namespace InPlayWise.Common.DTO
{
    public class LiveMatchBasicDto
    {
        public string MatchId { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public string MatchMinutes { get; set; }
        public string CompetitionName { get; set; }
    }
}
