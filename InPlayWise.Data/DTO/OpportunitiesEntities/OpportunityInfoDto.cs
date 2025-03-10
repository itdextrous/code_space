using System;

namespace InPlayWise.Data.DTO.OpportunitiesEntities
{
    public class OpportunityInfoDto
    {
        public string Name { get; set; }
        public bool isRelevant { get; set; } = true;
        public int Prediction { get; set; }



        public OpportunityInfoDto()
        {
            isRelevant = true;
            Prediction = new Random().Next(21, 89);
        }
    }
}
