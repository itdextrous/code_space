using System.Text.Json.Serialization;

namespace InPlayWise.Common.DTO.SportsApiResponseModels
{
	public class ApiTeam
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("competition_id")]
		public string CompetitionId { get; set; }

		[JsonPropertyName("country_id")]
		public string CountryId { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("short_name")]
		public string ShortName { get; set; }

		[JsonPropertyName("logo")]
		public string Logo { get; set; }

		[JsonPropertyName("national")]
		public int National { get; set; }

		[JsonPropertyName("country_logo")]
		public string CountryLogo { get; set; }

		[JsonPropertyName("foundation_time")]
		public long FoundationTime { get; set; }

		[JsonPropertyName("website")]
		public string WebSite { get; set; }

		[JsonPropertyName("coach_id")]
		public string CoachId { get; set; }

		[JsonPropertyName("venue_id")]
		public string VenueId { get; set; }

		[JsonPropertyName("market_value")]
		public long MarketValue { get; set; }

		[JsonPropertyName("market_value_currency")]
		public string MarketValueCurrency { get; set; }

		[JsonPropertyName("totoal_players")]
		public int TotalPlayers { get; set; }

		[JsonPropertyName("foreign_players")]
		public int ForeignPlayers { get; set; }

		[JsonPropertyName("national_players")]
		public int NationalPlayers { get; set; }

		[JsonPropertyName("updated_at")]
		public long UpdatedAt { get; set; }


	}
}
