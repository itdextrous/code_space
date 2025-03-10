using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class LeagueStatsCount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string LeagueId { get; set; }
        public ApplicationUser User { get; set; }
        public Competition Competition { get; set; }

    }
}
