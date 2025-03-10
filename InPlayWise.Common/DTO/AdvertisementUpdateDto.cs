using Microsoft.AspNetCore.Http;

namespace InPlayWise.Common.DTO
{
	public class AdvertisementUpdateDto
	{
		public string Id { get; set; }
		public string FirmLink { get; set; }
		public IFormFile Img { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}
