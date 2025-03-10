using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Data.Entities.FeaturesCountEntities
{
    public class Hedge
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int Count { get; set; }

        public ApplicationUser User { get; set; }
    }
}
