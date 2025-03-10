namespace InPlayWise.Common.DTO
{
    public class LiveMatchFilterDto
    {
        public string MatchId { get; set; }
        public string CompetitionName { get; set; }
        public int CompetitionType { get; set; }
        public List<string> CleverLabels { get; set; }
    }
}
