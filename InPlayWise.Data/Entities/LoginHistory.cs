using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities
{
    public class LoginHistory
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
