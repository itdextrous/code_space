using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.Entities
{
    public class FavouriteCompetitions
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CompetitionId { get; set; }
        public ApplicationUser User { get; set; }
        public Competition Competition { get; set; }
    }
}
