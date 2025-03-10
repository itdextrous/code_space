using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities
{
    public class OddsCompanyId
    {
        [Key]
        public string Id { get; set; }
        public int StatusCode { get; set; }
        public string Description { get; set; }
    }
}
