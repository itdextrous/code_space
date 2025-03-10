using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Common.DTO
{
    public class PriceResponseDto
    {
        public Guid Id { get; set; }
        public int PriceInCents { get; set; }
        public int ValidityInDays { get; set; }

    }
}
