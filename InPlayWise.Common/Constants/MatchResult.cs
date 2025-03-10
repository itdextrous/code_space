namespace InPlayWise.Common.Constants
{
	public class MatchResult
	{
		public bool Draw { get; set; }
		public bool HomeTeamComeBackToDraw { get; set; }
		public bool AwayTeamComeBackToDraw { get; set; }

		public bool HomeTeamWin { get; set; }
		//public bool HomeTeamComebackToWin { get; set; }
		public bool AwayTeamWin { get; set; }
		//public bool AwayTeamComebackToWin { get; set; }
		//public bool ScoredFirstAndWin { get; set; }

		public bool ComeBackToWin { get; set; }
	}
}
