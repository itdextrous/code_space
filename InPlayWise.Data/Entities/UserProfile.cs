using System.ComponentModel.DataAnnotations;
using InPlayWise.Data.Entities.FeaturesCountEntities;

namespace InPlayWise.Data.Entities
{
    public class UserProfile
    {
        [Key]
        public string UserId { get; set; }
        public bool ThemeDark { get; set; } = true ;
        public ApplicationUser User { get; set; }
        public FeatureCounter Features { get; set; }

    }
}