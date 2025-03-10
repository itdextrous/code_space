using InPlayWise.Data.Entities;

namespace InPlayWise.Data.DTO
{
    public class MatchShocksResponseDto
    {

        public MatchShocksResponseDto()
        {
            HomeTeamShocks = new List<Shocks>();
            AwayTeamShocks = new List<Shocks>();
        }

        public List<Shocks> HomeTeamShocks { get; set; }
        public List<Shocks> AwayTeamShocks { get; set; }

    }
}
