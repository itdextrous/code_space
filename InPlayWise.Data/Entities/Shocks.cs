using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;

namespace InPlayWise.Data.Entities
{
    public class Shocks
    {
        public Guid Id { get; set; }
        public string TeamId { get; set; }
        public string MatchId { get; set; }

        public string Description { get; set; }

        public LiveMatchModel Match { get; set; }
        public Team Team { get; set; }
        
    }
}
