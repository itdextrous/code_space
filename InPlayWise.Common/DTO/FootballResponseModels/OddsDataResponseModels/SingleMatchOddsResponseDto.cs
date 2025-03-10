namespace InPlayWise.Common.DTO.FootballResponseModels.OddsDataResponseModels
{
    public class SingleMatchOddsResponseDto
    {
        public long ChangeTime { get; set; }
        public string MatchTime { get; set; }
        public float HomeWin { get; set; }
        public float HandiCap { get; set; }
        public float AwayWin { get; set; }
        public int MatchStatus { get; set; }
        public bool SealTheDisk { get; set; }
        public string Score { get; set; }

    }
}
