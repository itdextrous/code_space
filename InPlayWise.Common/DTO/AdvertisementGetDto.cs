using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWise.Common.DTO
{
	public class AdvertisementGetDto
	{
		public string FirmLink { get; set; }
		public byte[] Img { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}
