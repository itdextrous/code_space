namespace InPlayWise.Common.Constants
{
	public static class ApiRoutes
	{

		public const string baseUrl = "https://api.thesports.com/v1/football/";

		// Basic Info Urls
		public const string Category = baseUrl + "category/list?";
		public const string Country = baseUrl + "country/list?";
		public const string Competition = baseUrl + "competition/additional/list?";
		public const string Team = baseUrl + "team/additional/list?";
		public const string Player = baseUrl + "player/with_stat/list/";
		public const string Coach = baseUrl + "coach/list?";
		public const string Referee = baseUrl + "referee/list?";
		public const string Venue = baseUrl + "venue/list?";
		public const string Season = baseUrl + "season/list?";
		public const string Stage = baseUrl + "stage/list?";

		// basic data
		public const string RecentMatch = baseUrl + "match/recent/list?";
		public const string RealTimeData = baseUrl + "match/detail_live?";
		public const string HistoricalStats = baseUrl + "match/live/history?";
		public const string SeasonResult = baseUrl + "match/season/recent?";
		public const string ScheduleAndResultsDateQuery = baseUrl + "match/diary?";


    }
}
