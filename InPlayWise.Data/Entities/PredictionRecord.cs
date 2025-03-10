namespace InPlayWise.Data.Entities
{
    public class PredictionRecord
    {
        public Guid Id { get; set; }

        public string MatchId { get; set; }

        public int MatchMinute { get; set; }

        public string OpportunityName { get; set; }

        public float PredictionProb { get; set; }


    }
}
