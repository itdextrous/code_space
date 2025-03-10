using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class FeatureCounter
    {
        [Key]
        public string UserId { get; set; }
        public List<LiveInsightsPerGame> LiveInsightsPerGame { get; set; }
        //public int ShockDetectors { get; set; }
        //public int CleverLabelling { get; set; }
        //public int AccumulatorHistory { get; set; }
        //public int Hedge { get; set; }
        //public int LeagueStats { get; set; }
        public UserProfile UserProfile { get; set; }

    }
}