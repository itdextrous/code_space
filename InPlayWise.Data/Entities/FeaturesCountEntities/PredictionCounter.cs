using InPlayWise.Data.SportsEntities;
using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class PredictionCounter
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string MatchId { get; set; }
        public int Hits { get; set; }
        public LiveMatchModel Match { get; set; }
        public ApplicationUser User { get; set; }
    }
}
