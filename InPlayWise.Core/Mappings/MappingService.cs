using InPlayWise.Common.DTO;
using InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.Services.FootballServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.Prediction;
using InPlayWise.Data.Entities.PredictionEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;
using Competition = InPlayWise.Data.Entities.SportsEntities.Competition;

namespace InPlayWise.Core.Mappings
{
    public class MappingService
    {
		public static BroadcastMatchDto LiveMatchToBroadCastDto(LiveMatchDto match)
		{
			return new BroadcastMatchDto()
			{
				MatchId = match.MatchId,
				MatchStatus = match.MatchStatus,
				MatchMinutes = match.MatchMinutes,
				HomeGoals = match.HomeGoals,
				AwayGoals = match.AwayGoals,
				HomeRed = match.HomeRed,
				AwayRed = match.AwayRed,
				HomeYellow = match.HomeYellow,
				AwayYellow = match.AwayYellow,
				HomeCorners = match.HomeCorners,
				AwayCorners = match.AwayCorners,
				HomeShotsOnTarget = match.HomeShotsOnTarget,
				HomeShotsOffTarget = match.HomeShotsOffTarget,
				AwayShotsOnTarget = match.AwayShotsOnTarget,
				AwayShotsOffTarget = match.AwayShotsOffTarget,
				HomeDangerousAttacks = match.HomeDangerousAttacks,
				AwayDangerousAttacks = match.AwayDangerousAttacks,
				HomeAttacks = match.HomeAttacks,
				AwayAttacks = match.AwayAttacks,
				HomePenalties = match.HomePenalties,
				AwayPenalties = match.AwayPenalties,
				HomePossession = match.HomePossession,
				AwayPossession = match.AwayPossession,
				HomeOwnGoals = match.HomeOwnGoals,
				AwayOwnGoals = match.AwayOwnGoals,
				HomeGoalMinutes = match.HomeGoalMinutes,
				AwayGoalMinutes = match.AwayGoalMinutes,
				HomeSubstitutionMinutes = match.HomeSubstitutionMinutes,
				HomeSubstitutionNames = match.HomeSubstitutionNames,
				AwaySubstitutionMinutes = match.AwaySubstitutionMinutes,
				AwaySubstitutionNames = match.AwaySubstitutionNames,
				HomeRedMinutes = match.HomeRedMinutes,
				HomeRedNames = match.HomeRedNames,
				AwayRedMinutes = match.AwayRedMinutes,
				AwayRedNames = match.AwayRedNames,
				HomeYellowMinutes = match.HomeYellowMinutes,
				HomeYellowNames = match.HomeYellowNames,
				AwayYellowMinutes = match.AwayYellowMinutes,
				AwayYellowNames = match.AwayYellowNames,
				HomeCornerMinutes = match.HomeCornerMinutes,
				AwayCornerMinutes = match.AwayCornerMinutes,
				HomeShotsOnTargetMinutes = match.HomeShotsOnTargetMinutes,
				AwayShotsOnTargetMinutes = match.AwayShotsOnTargetMinutes,
				HomeShotsMinutes = match.HomeShotsMinutes,
				AwayShotsMinutes = match.AwayShotsMinutes,
				HomeScorers = match.HomeScorers,
				AwayScorers = match.AwayScorers,
				HomePenaltiesRecord = match.HomePenaltiesRecord,
				AwayPenaltiesRecord = match.AwayPenaltiesRecord,
				OverTime = match.OverTime,
				PenaltyShootOut = match.PenaltyShootOut,
				HomeTeamHalfTimeScore = match.HomeTeamHalfTimeScore,
				HomeTeamOverTimeScore = match.HomeTeamOverTimeScore,
				HomeTeamPenaltyShootOutScore = match.HomeTeamPenaltyShootOutScore,
				AwayTeamHalfTimeScore = match.AwayTeamHalfTimeScore,
				AwayTeamOverTimeScore = match.AwayTeamOverTimeScore,
				AwayTeamPenaltyShootoutScore = match.AwayTeamPenaltyShootoutScore,
				NumOfOpportunities = match.NumOfOpportunities,
			};
		}
        public static Competition MapCompetitionResponseDtoToCompetition(CompetitionResponseDto comp)
        {
            return new Competition()
            {
                Id = comp.Id,
                Name = comp.Name,
                ShortName = comp.ShortName,
                CountryId = comp.CountryId,
                CurrentRound = comp.CurrentRound,
                CurrentSeasonId = comp.CurrentSeasonId,
                CurrentStageId = comp.CurrentStageId,
                Logo = comp.Logo,
                TotalRounds = comp.TotalRounds,
                PrimaryColor = comp.PrimaryColor,
                SecondaryColor = comp.SecondaryColor,
                LeagueLevel = comp.LeagueLevel
            };
        }

        public static RecentMatchModel MapLiveToRecent(LiveMatchModel match)
        {
            var rMatch = new RecentMatchModel()
            {
                MatchId = match.MatchId,
                HomeTeamId = match.HomeTeamId,
                AwayTeamId = match.AwayTeamId,
                SeasonId = match.SeasonId,
                HomeWin = match.HomeGoals > match.AwayGoals,
                HomeGoals = match.HomeGoals,
                AwayGoals = match.AwayGoals,
                GameDrawn = match.HomeGoals == match.AwayGoals,
                HomeCorners = match.HomeCorners,
                AwayCorners = match.AwayCorners,
                HomeRedCards = match.HomeRed,
                HomeYellowCards = match.HomeYellow,
                AwayRedCards = match.AwayRed,
                AwayYellowCards = match.AwayYellow,
                HomeGoalMinutes = match.HomeGoalMinutes,
                AwayGoalMinutes = match.AwayGoalMinutes,
                HomeShotsOnTarget = match.HomeShotsOnTarget,
                AwayShotsOnTarget = match.AwayShotsOnTarget,
                HomeShotsOffTarget = match.HomeShotsOffTarget,
                AwayShotsOffTarget = match.AwayShotsOffTarget,
                HomeAttacks = match.HomeAttacks,
                AwayAttacks = match.AwayAttacks,
                HomeDangerousAttacks = match.HomeDangerousAttacks,
                AwayDangerousAttacks = match.AwayDangerousAttacks,
                HomePenalties = match.HomePenalties,
                AwayPenalties = match.AwayPenalties,
                HomePossession = match.HomePossession,
                AwayPossession = match.AwayPossession,
                HomeOwnGoals = match.HomeOwnGoals,
                AwayOwnGoals = match.AwayOwnGoals,
                HomeSubstitutionMinutes = match.HomeSubstitutionMinutes,
                AwaySubstitutionMinutes = match.AwaySubstitutionMinutes,
                HomeRedMinutes = match.HomeRedMinutes,
                AwayRedMinutes = match.AwayRedMinutes,
                HomeYellowMinutes = match.HomeYellowMinutes,
                AwayYellowMinutes = match.AwayYellowMinutes,
                HomeCornerMinutes = match.HomeCornerMinutes,
                AwayCornerMinutes = match.AwayCornerMinutes,
                HomeShotsOnTargetMinutes = match.HomeShotsOnTargetMinutes,
                AwayShotsOnTargetMinutes = match.AwayShotsOnTargetMinutes,
                HomeShotsMinutes = match.HomeShotsMinutes,
                AwayShotsMinutes = match.AwayShotsMinutes,
                MatchStartTimeOfficial = match.MatchStartTimeOfficial,
                OverTime = match.OverTime,
                PenaltyShootout = match.PenaltyShootOut,
                HomeTeamHalfTimeScore = match.HomeTeamHalfTimeScore,
                AwayTeamHalfTimeScore = match.AwayTeamHalfTimeScore,
                HomeScorers = match.HomeScorers,
                AwayScorers = match.AwayScorers,
				HomePenaltiesRecord = match.HomePenaltiesRecord,
				AwayPenaltiesRecord = match.AwayPenaltiesRecord,
				HomeYellowNames = match.HomeYellowNames,
				AwayYellowNames =match.AwayYellowNames,
				HomeRedNames = match.HomeRedNames,
				AwayRedNames =match.AwayRedNames,
				HomeSubstituteNames= match.HomeSubstituteNames,
				AwaySubstituteNames =match.AwaySubstituteNames,
				StatsComplete = match.StatsComplete
            };
            return rMatch;
        }


		public static LiveMatchBasicDto MapLiveModelToLiveBasicModel(LiveMatchModel match)
		{
			return new LiveMatchBasicDto()
			{
				MatchId = match.MatchId,
				HomeTeamName = match.HomeTeam.Name,
				AwayTeamName = match.AwayTeam.Name,
				HomeGoals = match.HomeGoals,
				AwayGoals = match.AwayGoals,
				CompetitionName = match.Competition.Name,
				MatchMinutes = match.MatchMinutes.ToString()
			};
		}


        public static AllPrediction MapDailyPredictionToAllPrediction(DailyPrediction dt)
        {
            return new AllPrediction()
            {
                HomeGoals = dt.HomeGoals ,
                HomeShotsOnTarget = dt.HomeShotsOnTarget ,
                HomeShotsOffTarget = dt.HomeShotsOffTarget ,
                HomeAttacks = dt.HomeAttacks ,
                HomeDangerousAttacks = dt.HomeDangerousAttacks ,
                HomeCorners = dt.HomeCorners ,
                HomePenalties = dt.HomePenalties ,
                AwayGoals = dt.AwayGoals ,
                AwayShotsOnTarget = dt.AwayShotsOnTarget ,
                AwayShotsOffTarget = dt.AwayShotsOffTarget ,
                AwayAttacks = dt.AwayAttacks ,
                AwayDangerousAttacks = dt.AwayDangerousAttacks ,
                AwayCorners = dt.AwayCorners ,
                AwayPenalties = dt.AwayPenalties ,
                MatchMinutes = dt.MatchMinutes ,
                TotalScore = dt.TotalScore 
            };
        }


        public static DailyPredictionDataset MapAllPredictionDatasetToDailyPredictionDataset(AllPredictionDataset dt)
        {
            return new DailyPredictionDataset()
            {
                HomeGoals = dt.HomeGoals,
                HomeShotsOnTarget = dt.HomeShotsOnTarget,
                HomeShotsOffTarget = dt.HomeShotsOffTarget,
                HomeAttacks = dt.HomeAttacks,
                HomeDangerousAttacks = dt.HomeDangerousAttacks,
                HomeCorners = dt.HomeCorners,
                HomePenalties = dt.HomePenalties,
                AwayGoals = dt.AwayGoals,
                AwayShotsOnTarget = dt.AwayShotsOnTarget,
                AwayShotsOffTarget = dt.AwayShotsOffTarget,
                AwayAttacks = dt.AwayAttacks,
                AwayDangerousAttacks = dt.AwayDangerousAttacks,
                AwayCorners = dt.AwayCorners,
                AwayPenalties = dt.AwayPenalties,
                MatchMinutes = dt.MatchMinutes,
                HomePossession = dt.HomePossession,
                AwayPossession = dt.AwayPossession,
                HomeRed = dt.HomeRed,
                AwayRed = dt.AwayRed,
                HomeYellow = dt.HomeYellow,
                AwayYellow = dt.AwayYellow,
                MatchTimeSeconds = dt.MatchTimeSeconds,
                Id = dt.Id
            };
        }








        public static PredictionActiveMatchesData MapLiveMatchDataToPredictionActiveMatchData(LiveMatchDto match)
        {
            return new PredictionActiveMatchesData()
            {
		          Id = Guid.NewGuid() ,
		          MatchId = match.MatchId ,
		          HomeTeamId = match.HomeTeamId ,
		          AwayTeamId = match.AwayTeamId ,
		          CompetitionId = match.CompetitionId ,
		          MatchTime = match.MatchStartTimeOfficial ,
		          MatchMinutes = match.MatchMinutes ,
		          HomeGoals = match.HomeGoals ,
		          AwayGoals = match.AwayGoals ,
		          HomeRed = match.HomeRed ,
		          AwayRed = match.AwayRed ,
		          HomeYellow = match.HomeYellow ,
		          AwayYellow = match.AwayYellow ,
		          HomeCorners = match.HomeCorners ,
		          AwayCorners = match.AwayCorners ,
		          HomeShotsOnTarget = match.HomeShotsOnTarget ,
		          HomeShotsOffTarget = match.HomeShotsOffTarget ,
		          AwayShotsOnTarget = match.AwayShotsOnTarget ,
		          AwayShotsOffTarget = match.AwayShotsOffTarget ,
		          HomeDangerousAttacks = match.HomeDangerousAttacks ,
		          AwayDangerousAttacks = match.AwayDangerousAttacks ,
		          HomeAttacks = match.HomeAttacks ,
		          AwayAttacks = match.AwayAttacks ,
		          HomePenalties = match.HomePenalties ,
		          AwayPenalties = match.AwayPenalties ,
		          HomePossession = match.HomePossession ,
		          AwayPossession = match.AwayPossession ,
		          HomeOwnGoals = match.HomeOwnGoals ,
		          AwayOwnGoals = match.AwayOwnGoals ,
		          HomeTeamRank = match.HomeTeamRank ,
		          AwayTeamRank = match.AwayTeamRank ,
		          HomeTeamHalfTimeScore = match.HomeTeamHalfTimeScore,
		          AwayTeamHalfTimeScore = match.AwayTeamHalfTimeScore
	        };
        }



		public static PredictionFullData MapActiveMatchDataToPredictionFullData(PredictionActiveMatchesData match)
		{
			return new PredictionFullData()
			{
				Id = Guid.NewGuid(),
				MatchId = match.MatchId,
				HomeTeamId = match.HomeTeamId,
				AwayTeamId = match.AwayTeamId,
				CompetitionId = match.CompetitionId,
				MatchTime = match.MatchTime,
				MatchMinutes = match.MatchMinutes,
				HomeGoals = match.HomeGoals,
				AwayGoals = match.AwayGoals,
				HomeRed = match.HomeRed,
				AwayRed = match.AwayRed,
				HomeYellow = match.HomeYellow,
				AwayYellow = match.AwayYellow,
				HomeCorners = match.HomeCorners,
				AwayCorners = match.AwayCorners,
				HomeShotsOnTarget = match.HomeShotsOnTarget,
				HomeShotsOffTarget = match.HomeShotsOffTarget,
				AwayShotsOnTarget = match.AwayShotsOnTarget,
				AwayShotsOffTarget = match.AwayShotsOffTarget,
				HomeDangerousAttacks = match.HomeDangerousAttacks,
				AwayDangerousAttacks = match.AwayDangerousAttacks,
				HomeAttacks = match.HomeAttacks,
				AwayAttacks = match.AwayAttacks,
				HomePenalties = match.HomePenalties,
				AwayPenalties = match.AwayPenalties,
				HomePossession = match.HomePossession,
				AwayPossession = match.AwayPossession,
				HomeOwnGoals = match.HomeOwnGoals,
				AwayOwnGoals = match.AwayOwnGoals,
				HomeTeamRank = match.HomeTeamRank,
				AwayTeamRank = match.AwayTeamRank,
				HomeTeamHalfTimeScore = match.HomeTeamHalfTimeScore,
				AwayTeamHalfTimeScore = match.AwayTeamHalfTimeScore
			};
		}


        public static void MapRecentMatchResponseDtoToRecentMatchMoel(RecentMatchModel m, RecentMatchResponseDto dto)
        {
            try
            {
                int homeScore = dto.HomeScores[0] + (dto.HomeScores[5] == 0 ? 0 : dto.HomeScores[5] - dto.HomeScores[0]) + dto.HomeScores[6];
                int awayScore = dto.AwayScores[0] + (dto.AwayScores[5] == 0 ? 0 : dto.AwayScores[5] - dto.AwayScores[0]) + dto.AwayScores[6];
                m.MatchId = dto.Id;
                m.HomeTeamId = dto.HomeTeamId;
                m.AwayTeamId = dto.AwayTeamId;
                m.CompetitionId = dto.CompetitionId;
                m.SeasonId = dto.SeasonId;
                m.HomeWin = homeScore > awayScore;
                m.HomeGoals = homeScore;
		        m.AwayGoals = awayScore;
		        m.GameDrawn = homeScore == awayScore;
		        m.HomeCorners = dto.HomeScores[4];
		        m.AwayCorners = dto.AwayScores[4];
		        m.HomeRedCards = dto.HomeScores[2];
		        m.HomeYellowCards = dto.HomeScores[3];
		        m.AwayRedCards = dto.HomeScores[2];
		        m.AwayYellowCards = dto.HomeScores[3];
		        m.Ended = dto.StatusId == 8;
		        m.AbruptEnd = dto.StatusId != 8;
		        m.HomeTeamHalfTimeScore = dto.HomeScores[1];
		        m.AwayTeamHalfTimeScore = dto.AwayScores[1];
                m.MatchStartTimeOfficial = DateTimeOffset.FromUnixTimeSeconds(dto.MatchTime).UtcDateTime;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }

		public static LiveMatchDto LiveMatchModelToDto(LiveMatchModel model)
		{
			return new LiveMatchDto
			{
				MatchId = model.MatchId,
				MatchStatus = model.MatchStatus,
				MatchMinutes = model.MatchMinutes,
				HomeGoals = model.HomeGoals,
				AwayGoals = model.AwayGoals,
				HomeRed = model.HomeRed,
				AwayRed = model.AwayRed,
				HomeYellow = model.HomeYellow,
				AwayYellow = model.AwayYellow,
				HomeCorners = model.HomeCorners,
				AwayCorners = model.AwayCorners,
				HomeShotsOnTarget = model.HomeShotsOnTarget,
				HomeShotsOffTarget = model.HomeShotsOffTarget,
				AwayShotsOnTarget = model.AwayShotsOnTarget,
				AwayShotsOffTarget = model.AwayShotsOffTarget,
				HomeDangerousAttacks = model.HomeDangerousAttacks,
				AwayDangerousAttacks = model.AwayDangerousAttacks,
				HomeAttacks = model.HomeAttacks,
				AwayAttacks = model.AwayAttacks,
				HomePenalties = model.HomePenalties,
				AwayPenalties = model.AwayPenalties,
				HomePossession = model.HomePossession,
				AwayPossession = model.AwayPossession,
				HomeOwnGoals = model.HomeOwnGoals,
				AwayOwnGoals = model.AwayOwnGoals,
				HomeGoalMinutes = model.HomeGoalMinutes,
				AwayGoalMinutes = model.AwayGoalMinutes,
				HomeSubstitutionMinutes = model.HomeSubstitutionMinutes,
				AwaySubstitutionMinutes = model.AwaySubstitutionMinutes,
				HomeRedMinutes = model.HomeRedMinutes,
				AwayRedMinutes = model.AwayRedMinutes,
				HomeYellowMinutes = model.HomeYellowMinutes,
				AwayYellowMinutes = model.AwayYellowMinutes,
				HomeCornerMinutes = model.HomeCornerMinutes,
				AwayCornerMinutes = model.AwayCornerMinutes,
				HomeShotsOnTargetMinutes = model.HomeShotsOnTargetMinutes,
				AwayShotsOnTargetMinutes = model.AwayShotsOnTargetMinutes,
				HomeShotsMinutes = model.HomeShotsMinutes,
				AwayShotsMinutes = model.AwayShotsMinutes,
				HomeScorers = model.HomeScorers,
				AwayScorers = model.AwayScorers,
				CompetitionId = model.CompetitionId,
				CompetitionName = model.Competition.Name,
				CompetitionShortName = model.Competition.ShortName,
				CompetitionLogo = model.Competition.Logo,
				SeasonId = model.SeasonId,
				HomeTeamId = model.HomeTeamId,
				AwayTeamId = model.AwayTeamId,
				HomeTeamName = model.HomeTeam.Name,
				AwayTeamName = model.AwayTeam.Name,
				HomeTeamShortName = model.HomeTeam.ShortName,
				AwayTeamShortName = model.AwayTeam.ShortName,
				HomeTeamLogo = model.HomeTeam.Logo,
				AwayTeamLogo = model.AwayTeam.Logo,
				MatchStartTimeOfficial = model.MatchStartTimeOfficial,
				OverTime = model.OverTime,
				PenaltyShootOut = model.PenaltyShootOut,
				HomeTeamRank = model.HomeTeamRank,
				AwayTeamRank = model.AwayTeamRank,
				HomeTeamHalfTimeScore = model.HomeTeamHalfTimeScore,
				HomeTeamOverTimeScore = model.HomeTeamOverTimeScore,
				HomeTeamPenaltyShootOutScore = model.HomeTeamPenaltyShootOutScore,
				AwayTeamHalfTimeScore = model.AwayTeamHalfTimeScore,
				AwayTeamOverTimeScore = model.AwayTeamOverTimeScore,
				AwayTeamPenaltyShootoutScore = model.AwayTeamPenaltyShootoutScore,
				NumOfOpportunities = model.NumOfOpportunities,
				RoundNumber = model.RoundNumber,
				GroupNumber = model.GroupNumber,
				StageName = model.StageName,
				HomePenaltiesRecord = model.HomePenaltiesRecord,
				AwayPenaltiesRecord = model.AwayPenaltiesRecord,
				HomeTeamLastMatchDate = model.HomeTeamLastMatchDate,
				AwayTeamLastMatchDate = model.AwayTeamLastMatchDate,
				CompetitionCategory = model.CompetititionCategory,
				StatsComplete = model.StatsComplete,
				HomeYellowNames = model.HomeYellowNames,
				AwayYellowNames = model.AwayYellowNames,
				HomeRedNames = model.HomeRedNames,
				AwayRedNames = model.AwayRedNames,
				HomeSubstitutionNames = model.HomeSubstituteNames,
				AwaySubstitutionNames = model.AwaySubstituteNames,

				
			};
		}

		public static HistoricalInsightsDto MapInsightsToHistoricalInsightsDto(Insights insights)
		{
			return new HistoricalInsightsDto
			{
				GoalsScoredAvg = insights.GoalsScoredAvg,
				GoalsScoredFirstHalfAvg = insights.GoalsScoredFirstHalfAvg,
				GoalsScoredSecondHalfAvg = insights.GoalsScoredSecondHalfAvg,
				GoalsConcededAvg = insights.GoalsConcededAvg,
				GoalsConcededFirstHalfAvg = insights.GoalsConcededFirstHalfAvg,
				GoalsConcededSecondHalfAvg = insights.GoalsConcededSecondHalfAvg,
				ScoredFirstHalfAndSecondHalfPercent = insights.ScoredFirstHalfAndSecondHalfPercent,
				GoalsConcededFirstHalfAndSecondHalfPercent = insights.GoalsConcededFirstHalfAndSecondHalfPercent,
				GoalsAvg = insights.GoalsAvg,
				GoalsFirstHalfAvg = insights.GoalsFirstHalfAvg,
				GoalsSecondHalfAvg = insights.GoalsSecondHalfAvg,
				GoalsFirstHalfAndSecondHalfPercent = insights.GoalsFirstHalfAndSecondHalfPercent,
				OverZeroPointFivePercent = insights.OverZeroPointFivePercent,
				OverOnePointFivePercent = insights.OverOnePointFivePercent,
				OverTwoPointFivePercent = insights.OverTwoPointFivePercent,
				OverThreePointFivePercent = insights.OverThreePointFivePercent,
				BothTeamsScoredPercent = insights.BothTeamsScoredPercent,
				NoGoalScoredPercent = insights.NoGoalScoredPercent,
				HomeWinPercent = insights.HomeWinPercent,
				AwayWinPercent = insights.AwayWinPercent,
				ShotsOnTarget = insights.ShotsOnTarget,
				DangerousAttack = insights.DangerousAttack,
				ShotsOnTargetAverage = insights.ShotsOnTargetAverage,
				DangerousAttacksAverage = insights.DangerousAttacksAverage,
				AverageCornersOfTeam = insights.AverageCornersOfTeam,
				AverageCornersInGame = insights.AverageCornersInGame,
				CleanSheetPercent = insights.CleanSheetPercent
			};
		}


		public static LiveInsightsDto MapToLiveInsightsDto(Insights insights)
		{
			return new LiveInsightsDto
			{
				Possession = insights.LivePossession,
				DangerousAttackPercentage = insights.LiveDangerousAttackPercentage,
				ShotsOnTargetPercentage = insights.LiveShotsOnTargetPercentage,
				Corners = insights.LiveCorners
			};
		}




		public static Competition ApiCompetitionToCompetitionModel(ApiCompetition dt)
		{
			try
			{
				return new Competition()
				{
					Id = dt.Id,
					Name = dt.Name,
					ShortName = dt.ShortName,
					Logo = dt.Logo,
					CurrentSeasonId = dt.CurrentSeasonId,
					CurrentStageId = dt.CurrentStageId,
					CurrentRound = dt.CurrentRound,
					TotalRounds = dt.RoundCount,
					CountryId = dt.CountryId,
					PrimaryColor = dt.PrimaryColor,
					SecondaryColor = dt.SecondaryColor,
					Type = dt.Type,
				
				};
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}

		public static Category ApiCategoryToEntity(ApiCategory apiCat)
		{
			try
			{
				return new Category()
				{
					Id = apiCat.Id,
					Name = apiCat.Name
				};
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public static Competition ApiCompetitionToEntity(ApiCompetition apiComp)
		{
			try
			{
				return new Competition()
				{
					Id = apiComp.Id,
					Name = apiComp.Name,
					ShortName = apiComp.ShortName,
					CountryId = apiComp.CountryId,
					Logo = apiComp.Logo,
					CurrentSeasonId = apiComp.CurrentSeasonId,
					CurrentStageId = apiComp.CurrentStageId,
					CurrentRound = apiComp.CurrentRound,
					TotalRounds = apiComp.RoundCount,
					PrimaryColor = apiComp.PrimaryColor,
					SecondaryColor = apiComp.SecondaryColor,
					Type = apiComp.Type
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}


		public static Country ApiCountryToEntity(ApiCountry apiCon)
		{
			try
			{
				return new Country()
				{
					Id = apiCon.Id,
					Name = apiCon.Name,
					Logo = apiCon.Logo,
					CategoryId = apiCon.CategoryId
				};
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public static RecentMatchResponseDto RecentMatchModelToDto()
		{
			return new RecentMatchResponseDto()
			{

			};
		}


		public static Season ApiSeasonToEntity(ApiSeason apiCon)
		{
			try
			{
				return new Season()
				{
					Id = apiCon.Id,
					CompetitionId = apiCon.CompetitionId,
					Year = apiCon.Year,
					HasPlayerStats = apiCon.HasPlayerStats == 1,
					HasTeamStats = apiCon.HasTeamStats == 1,
					IsCurrent = apiCon.IsCurrent == 1,
					StartTime = apiCon.StartTime,
					EndTime = apiCon.EndTime
				};
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public static Team ApiTeamToEntity(ApiTeam apiTeam)
		{
			try
			{
				return new Team()
				{
					Id = apiTeam.Id,
					CompetitionId = apiTeam.CompetitionId,
					CountryId = apiTeam.CountryId,
					Name = apiTeam.Name,
					ShortName = apiTeam.ShortName,
					Logo = apiTeam.Logo,
					IsNational = apiTeam.National == 1,
					CountryLogo = apiTeam.CountryLogo,
					FoundationTime = apiTeam.FoundationTime
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

		public static UpcomingMatch ApiRecentMatchToUpcomingEntity(ApiRecentMatch apiMatch)
		{
			try
			{
				return new UpcomingMatch()
				{
					Id = apiMatch.Id,
					HomeTeamId = apiMatch.HomeTeamId,
					AwayTeamId = apiMatch.AwayTeamId,
					CompetitionId = apiMatch.CompetitionId,
					SeasonId = apiMatch.SeasonId,
					HomeTeamRank = string.IsNullOrEmpty(apiMatch.HomePosition) ? 0 : int.Parse(apiMatch.HomePosition),
					AwayTeamRank = string.IsNullOrEmpty(apiMatch.AwayPosition) ? 0 : int.Parse(apiMatch.AwayPosition),
					time = DateTimeOffset.FromUnixTimeSeconds(apiMatch.MatchTime).DateTime,
					RoundNum = apiMatch.Round.RoundNum,
					GroupNum = apiMatch.Round.GroupNum
				};
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}
        

		public static Accumulator MapToAccumulator(OpportunityDto opportunityDto)
		{
			try
			{
				return new Accumulator()
				{
                   
                    OpportunityName = opportunityDto.OpportunityName,
                    ProbabilityPercentage = opportunityDto.ProbabilityPercentage,
					MatchId = opportunityDto.MatchId,
					Odds = opportunityDto.Odds,
					IsHedged = opportunityDto.IsHedged,
					ConfidencyLevel=opportunityDto.ConfidencyLevel

				};
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}

            public static OpportunityDto MapToOpportunityDto(HedgeDto hedge)
            {
                return new OpportunityDto
                {
                    OpportunityName = hedge.OpportunityName,
                    ProbabilityPercentage = hedge.ProbabilityPercentage,
                    MatchId = hedge.MatchId,
					Odds = hedge.Odds,
					ConfidencyLevel=hedge.ConfidencyLevel
                };
            }
		    public static HedgeResponseDto MaptoHedgeResponse(HedgeDto hedgeDto)
		    {
            try
            {
                return new HedgeResponseDto()
                {

                    OpportunityName = hedgeDto.OpportunityName,
                    ProbabilityPercentage = hedgeDto.ProbabilityPercentage,
                    MatchId = hedgeDto.MatchId,
					ConfidencyLevel= hedgeDto.ConfidencyLevel,
					Odds =hedgeDto.Odds

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        }
}