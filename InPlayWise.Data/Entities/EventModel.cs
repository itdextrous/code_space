using Chat.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace InPlayWiseData.Entities
{
    public class EventModel : BaseEntity
    {
        [Key]
        public Guid EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public string EventGround { get; set; }
    }
}
