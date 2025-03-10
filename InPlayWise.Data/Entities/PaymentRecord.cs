using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Data.Entities
{
    public class PaymentRecord
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public string CustomerEmail { get; set; }

        public string InvoiceId { get; set; }

        public int Amount { get; set; }

    }
}
