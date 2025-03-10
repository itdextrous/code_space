using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Common.DTO.CachedDto
{
    public class TeamCachedDto
    {
        public string Id { get; set; }
        public string CompetitionId { get; set; }
        public string CountryId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Logo { get; set; }
        public bool IsNational { get; set; }
        public string CountryLogo { get; set; }
        public long FoundationTime { get; set; }

    }
}
