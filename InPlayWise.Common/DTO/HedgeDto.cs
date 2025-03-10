using InPlayWise.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Common.DTO
{
    public class HedgeDto
    {
        public string OpportunityName { get; set; }
        public float ProbabilityPercentage { get; set; }
        public string MatchId { get; set; }

        public float Odds { get; set; }
        public int WillLose { get; set; }
        public ConfidencyLevel ConfidencyLevel { get; set; }
       
    }
}
