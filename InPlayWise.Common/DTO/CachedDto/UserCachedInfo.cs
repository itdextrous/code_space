namespace InPlayWise.Common.DTO.CachedDto
{
    public class UserCachedInfo
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string plan { get; set; }
        public List<List<OpportunityCachedDto>> Accumulators {  get; set; }
        public List<TeamCachedDto> FavouriteTeams { get ; set; }
    }
}
