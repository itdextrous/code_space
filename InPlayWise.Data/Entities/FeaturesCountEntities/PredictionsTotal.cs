using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class PredictionsTotal
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int Hits { get; set; }
        public ApplicationUser User { get; set; }
    }
}
