using InPlayWise.Data.SportsEntities;
using System.Text.Json.Serialization;

namespace InPlayWise.Data.Entities
{
    public class Opportunity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string MatchId { get; set; }

        public string Name { get; set; }

        public float Odds { get; set; }

        public float Prediction { get; set; }

        [JsonIgnore]
        public LiveMatchModel Match { get; set; }

    }
}
