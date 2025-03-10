using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities
{
    public class Advertisement
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string FirmLink { get; set; }

        [Required]
        public string ImgLink { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
