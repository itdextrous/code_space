using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InPlayWise.Common.DTO
{
    public class PlansJsonDto
    {
        [JsonPropertyName("plans")]
        public List<PlanJson> Plans { get; set; }
    }

    public class PlanJson
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("prices")]
        public List<PriceJson> Prices { get; set; }
        [JsonPropertyName("features")]
        public FeatureJson Features { get; set; }
    }

    public class PriceJson {
        [JsonPropertyName("durationInDays")]
        public int DurationInDays { get; set; }
        [JsonPropertyName("amountInCents")]
        public int AmountInCents {  get; set; }
        [JsonPropertyName("recurring")]
        public bool Recurring { get; set; }
    }

    public class FeatureJson
    {
        [JsonPropertyName("liveInsightsPerGame")]
        public int LiveInsightsPerGame { get; set; }

        [JsonPropertyName("livePredictionPerGame")]
        public int LivePredictionPerGame { get; set; }

        [JsonPropertyName("maxPredictions")]
        public int MaxPredictions { get; set; }

        [JsonPropertyName("accumulatorGenerator")]
        public int AccumulatorGenerator { get; set; }

        [JsonPropertyName("shockDetectors")]
        public int ShockDetectors { get; set; }

        [JsonPropertyName("cleverLabelling")]
        public int CleverLabelling { get; set; }

        [JsonPropertyName("historyOfAccumulator")]
        public int HistoryOfAccumulator { get; set; }

        [JsonPropertyName("wiseProHedge")]
        public int WiseProHedge { get; set; }

        [JsonPropertyName("leagueStatistics")]
        public int LeagueStatistics { get; set; }

        [JsonPropertyName("wiseProIncluded")]
        public bool WiseProIncluded { get; set; }
    }
}
