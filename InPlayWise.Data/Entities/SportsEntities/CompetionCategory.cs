using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InPlayWise.Data.Entities.SportsEntities
{
    public class CompetionCategory

    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<Competition> Competitions { get; set; }
    }
}
