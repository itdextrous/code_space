using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Data.Entities
{
    public class ValidateEmailModel
    {

        [Key]
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime RequestedOn { get; set; }

    }
}
