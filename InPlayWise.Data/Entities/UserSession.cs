using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Data.Entities
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime ConnectedTime { get; set; } 
        public DateTime DisconnectedTime { get; set; }

        [NotMapped]
        public string ProductId { get; set; }
    }
}
