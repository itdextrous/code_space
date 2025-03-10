using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.Entities
{
    public class MatchAlert
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string MatchId { get; set; }
        public DateTime AlertTime { get; set; }

        public bool EmailAlert { get; set; }
        public bool NotificationAlert { get; set; }

        public UpcomingMatch UpcomingMatch { get; set; }
        public ApplicationUser User { get; set; }

        

    }
}
