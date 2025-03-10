using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.Entities
{
    public class FavouriteTeams
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Team Teams { get; set; }
        public string TeamId { get; set; }
    }
}
