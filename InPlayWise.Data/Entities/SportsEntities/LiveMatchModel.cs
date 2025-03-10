using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.Prediction;
using InPlayWise.Data.Entities.SportsEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InPlayWise.Data.SportsEntities
{
    public class LiveMatchModel
	{
		[Key]
		public string MatchId { get; set; } = "";
		public int MatchStatus { get; set; }
		public int MatchMinutes { get; set; }
		public int HomeGoals { get; set; }
		public int AwayGoals { get; set; }
		public int HomeRed { get; set; }
		public int AwayRed { get; set; }
		public int HomeYellow { get; set; }
		public int AwayYellow { get; set; }
		public int HomeCorners { get; set; }
		public int AwayCorners { get; set; }
		public int HomeShotsOnTarget { get; set; }
		public int HomeShotsOffTarget { get; set; }
		public int AwayShotsOnTarget { get; set; }
		public int AwayShotsOffTarget { get; set; }
		public int HomeDangerousAttacks { get; set; }
		public int AwayDangerousAttacks { get; set; }
		public int HomeAttacks { get; set; }
		public int AwayAttacks { get; set; }
		public int HomePenalties { get; set;}
		public int AwayPenalties { get; set; }
		public int HomePossession { get; set; }
		public int AwayPossession { get; set; }
		public int HomeOwnGoals { get; set; }
		public int AwayOwnGoals { get; set; }
		public string HomeGoalMinutes { get; set; } = "" ;
		public string AwayGoalMinutes { get; set; } = "" ;
		public string HomeSubstitutionMinutes { get; set; } = "";
		public string AwaySubstitutionMinutes { get; set; } = "";
		public string HomeRedMinutes { get; set; } = "";
		public string AwayRedMinutes { get; set; } = "" ;
		public string HomeYellowMinutes { get; set; } = "";
		public string AwayYellowMinutes { get; set; } = "";
		public string HomeCornerMinutes { get; set; } = "";
		public string AwayCornerMinutes { get; set; } = "";
		public string HomeShotsOnTargetMinutes { get; set; } = "";
		public string AwayShotsOnTargetMinutes { get; set; } = "";
		public string HomeShotsMinutes { get; set; } = "";
		public string AwayShotsMinutes { get; set; } = "";
		public string HomeScorers { get; set; } = "";
		public string AwayScorers { get; set; } = "";
		public string CompetitionId { get; set; } = "";
		public string SeasonId { get; set; } = "";
		public string HomeTeamId { get; set; } = "";
		public string AwayTeamId { get; set; } = "";
		public long MatchStartTime { get; set; }
		public long CurrentKickoffTime { get; set; }
		public DateTime MatchStartTimeOfficial { get; set; }
		public bool OverTime { get; set; }
		public bool PenaltyShootOut { get; set; }
		public int HomeTeamRank { get; set; } 
		public int AwayTeamRank { get; set; } 
		public int HomeTeamHalfTimeScore { get; set; }
		public int HomeTeamOverTimeScore { get; set; }
		public int HomeTeamPenaltyShootOutScore { get; set; }
		public int AwayTeamHalfTimeScore { get; set; }
		public int AwayTeamOverTimeScore { get; set; }
		public int AwayTeamPenaltyShootoutScore { get; set; }
		public int NumOfOpportunities { get; set; }
		public int RoundNumber { get; set; }
		public int GroupNumber { get; set; }
		public string StageName { get; set; } = "";
		public string HomePenaltiesRecord { get; set; } = "";
		public string AwayPenaltiesRecord { get; set; } = "";

		public bool StatsComplete { get; set; } = false;
		public string HomeSubstituteNames { get; set; } = "";
		public string AwaySubstituteNames { get; set; } = "";
		public string HomeYellowNames { get; set; } = "";
		public string AwayYellowNames { get; set; } = "";
		public string HomeRedNames { get; set; } = "";
		public string AwayRedNames { get; set; } = "";
		public string OwnGoalMinutes { get; set; } = "";

		public DateTime HomeTeamLastMatchDate { get; set; }
		public DateTime AwayTeamLastMatchDate { get; set; }

		public bool Ended { get; set; }

		public string CompetititionCategory { get; set; } = "";
		public int CompetitionType { get; set; } 

		public Team HomeTeam { get; set; }

		public Team AwayTeam { get; set; }

		public Competition Competition { get; set; }


		[JsonIgnore]
		public List<LiveInsightsPerGame> LiveInsightsCounter { get; set; }

		[JsonIgnore]
		public Opportunities OpportunitiesAll { get; set; }

		[JsonIgnore]
		public List<Opportunity> Opportunities { get; set; }

		[JsonIgnore]
		public List<Shocks> Shocks { get; set; }

		[JsonIgnore]
		public List<ShockCounter> ShockCounter { get; set; }

		[JsonIgnore]
		public List<PredictionCounter> PredictionCounter { get; set; }

		[JsonIgnore]
		public List<PredictionActiveMatchesData> PredictionActiveMatches { get; set;}
		

	}
}