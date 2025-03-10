namespace InPlayWiseData.Entities
{
    public class PredictionResult
    {
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }

    }

    public class PredictionResultProbability
    {
        public float Probability { get; set; }

    }
}
