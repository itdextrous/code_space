using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Common.DTO
{
    public class MatchAccuracyDto
    {
        public string MatchId { get; set; }
        public string Opportunity { get; set; }
        public float Accuracy { get; set; }
    }
}
