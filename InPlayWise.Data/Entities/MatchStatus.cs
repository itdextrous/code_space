using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities
{
    /// <summary>
    /// A static entity which store in database
    /// </summary>
    public class MatchStatus
    {
        [Key()]
        public string Id { get; set; }
        public int StatusCode { get; set; }
        public string Description { get; set; }
    }
}
