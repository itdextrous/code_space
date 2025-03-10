using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Data.DTO
{
    public class HistoricalStatsDto
    {
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public byte AvgPossession { get; set; }
        List<string> MatchStats { get; set; }
        public List<bool> ScoredFirstAndWon { get; set; } = new List<bool>();
        public byte AvgGoalsScoredMin { get; set; }
        public byte AvgShotsOnGoal { get; set; }
        public byte AvgShotsOffGoal { get; set; }
        public byte Goals { get; set; }
        public byte GoalsFirstHalf { get; set; }
        public byte GoalsSecondHalf { get; set; }
        public byte ShotsConceded { get; set; }
        public List<bool> Penalties { get; set; }
        public byte RedCards { get; set; }
        public byte YellowCards { get; set; }


        public HistoricalStatsDto(List<RecentMatchModel> matches, Team team) {

            bool homeTeam = matches[0].HomeTeamId.Equals(team.Id);
            TeamId = team.Id;
            TeamName = team.Name;
            TeamLogo = team.Logo;

            foreach(var m in matches)
            {
                AvgPossession = (byte)(homeTeam ? m.HomePossession / 3 : m.AwayPossession / 3);

                if (m.GameDrawn)
                {
                    MatchStats.Add("d");
                }
                else if (homeTeam)
                {
                    if (m.HomeWin)
                        MatchStats.Add("w");
                    else
                        MatchStats.Add("l");
                }
                else if (!homeTeam)
                {
                    if (!m.HomeWin)
                        MatchStats.Add("w");
                    else
                        MatchStats.Add("l");
                }

                ScoredFirstAndWon.Add(false);


                if (homeTeam)
                {
                    AvgGoalsScoredMin += (byte)(m.OverTime ? 120 / m.HomeGoals : 90 / m.HomeGoals);
                }
                else
                {
                    AvgGoalsScoredMin += (byte)(m.OverTime ? 120 / m.AwayGoals : 90 / m.AwayGoals);
                }

            }
            
            

            
        
        }



    }
}
