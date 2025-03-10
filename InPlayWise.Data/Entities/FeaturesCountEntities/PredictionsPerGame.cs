using InPlayWise.Data.SportsEntities;
using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class PredictionsPerGame
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string GameId { get; set; }
        public int Hits { get; set; }
        public LiveMatchModel Match { get; set; }
        public ApplicationUser User { get; set; }
    }
}
