namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class AccumulatorGenerators
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public int Hits { get; set; }
        public ApplicationUser User { get; set; }
    }
}
