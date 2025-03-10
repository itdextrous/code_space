using Microsoft.AspNetCore.Http;

namespace InPlayWise.Common.DTO
{
    public class AdvertisementDto
    {
        public string FirmLink { get; set; }
        public IFormFile Img { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
