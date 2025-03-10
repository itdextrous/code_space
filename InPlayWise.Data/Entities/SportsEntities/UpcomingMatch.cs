using System.Text.Json.Serialization;

namespace InPlayWise.Data.Entities.SportsEntities
{
    public class UpcomingMatch
    {

        public string Id { get; set; }
        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }
        public string CompetitionId { get; set; }
        public string SeasonId { get; set; }
        public int HomeTeamRank { get; set; }
        public int AwayTeamRank { get; set; }
        public DateTime time { get; set; }
        public int RoundNum { get; set; }
        public int GroupNum { get; set; }
        public string StageName { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Competition Competition { get; set; }

        public DateTime HomeTeamLastGame { get; set; }
        public DateTime AwayTeamLastGame { get; set; }


        [JsonIgnore]
        public List<MatchAlert> MatchAlerts { get; set; }



    }
}
