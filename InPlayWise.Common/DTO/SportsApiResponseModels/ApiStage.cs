namespace InPlayWise.Common.DTO.SportsApiResponseModels
{
	public class ApiStage
	{
		public string Id { get; set; }
		public string SeasonId { get; set; }
		public string Name { get; set; }
		public int Mode { get; set; }
		public int GroupCount { get; set; }
		public int RoundCount { get; set; }
		public int Order {  get; set; }
		public long UpdatedAt { get; set; }
	}
}
