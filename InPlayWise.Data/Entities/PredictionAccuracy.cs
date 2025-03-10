using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Data.Entities
{
    public  class PredictionAccuracy
    {
        public new Guid Id { get; set; }
        public string MatchId { get; set; }
        public DateTime EndTime { get; set; }
        public string Opportunity { get; set; }
        public float Prediction {  get; set; }
        public float Accuracy { get; set; } 
    }
}
