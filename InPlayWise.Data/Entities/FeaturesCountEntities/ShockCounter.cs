using InPlayWise.Data.SportsEntities;
using System.Text.Json.Serialization;

namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class ShockCounter
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string Matchid { get; set;  }

        [JsonIgnore]
        public ApplicationUser User { get; set; }

        [JsonIgnore]
        public LiveMatchModel Match { get; set; }
        public int Count { get; set; }

        //For counting total shockdetectors just add the userid for a match
    }
}
