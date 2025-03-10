using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class CleverLabelsCounter
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string TeamId { get; set; }
        public ApplicationUser User { get; set; }
        public Team Team { get; set; }

    }
}