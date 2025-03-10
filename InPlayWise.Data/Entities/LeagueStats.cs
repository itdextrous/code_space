using InPlayWise.Data.Entities.SportsEntities;
using System.ComponentModel.DataAnnotations;

namespace InPlayWise.Data.Entities
{
	public class LeagueStats
	{
		[Key]
		public string CompetitionId { get; set; }
		public string CompetitionLogo { get; set; }
		public float AvgGamesPlayed { get; set; }
		public string SeasonId { get; set; }
		public int OverOnePointFive { get; set; }
		public int OverTwoPointFive { get ; set; }
		public int OverThreePointFive { get; set; }
		public float AvgCardsRed { get; set; }
		public float AvgCardsYellow { get; set; }
		public int Draws { get; set; }
		public int BTTS { get; set; }
		public float GoalsPerMatch { get; set; }
		public float CardsPerMatch { get; set; }
		public float CornersPerMatch { get; set; }
		public int MatchesThisSeason { get; set; }


		public DateTime SeasonStartDate { get; set; }
		public DateTime SeasonEndDate { get; set; }

		public DateTime UpdatedTime { get; set; }

		public Competition Competition { get; set; }

	}
}
