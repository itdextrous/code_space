namespace InPlayWise.Common.DTO
{
    public class UpcomingMatchesSearchResponseDto
    {
        public string Id { get; set; }
        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }
        public string CompetitionId { get; set; }
        public int HomeTeamRank { get; set; }
        public int AwayTeamRank { get; set; }
        public DateTime Time { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string CompetitionName { get; set;}
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string CompetitionLogo { get; set; }

    }
}
