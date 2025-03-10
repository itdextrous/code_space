namespace InPlayWise.Common.DTO
{
    public class MatchAlertRequestDto
    {
        public string MatchId { get; set; }
        public short AlertBeforeInMinutes { get; set; }

        public DateTime MatchTime { get; set; }
        
    }
}
