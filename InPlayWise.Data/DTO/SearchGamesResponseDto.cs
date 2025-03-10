using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.DTO
{
    public class SearchGamesResponseDto
    {
        public List<LiveMatchDto> LiveMatches { get; set; } = new List<LiveMatchDto>();
        public List<UpcomingMatch> UpcomingMatches { get; set; } = new List<UpcomingMatch>();
        public List<RecentMatchModel> RecentMatches { get; set; } = new List<RecentMatchModel>();
    }
}
