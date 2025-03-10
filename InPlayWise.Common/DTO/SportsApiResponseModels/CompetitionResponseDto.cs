namespace InPlayWise.Common.DTO.SportsApiResponseModels
{
    public class CompetitionResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Logo { get; set; }
        public string CurrentSeasonId { get; set; }
        public string CurrentStageId { get; set; }
        public int CurrentRound { get; set; }
        public int TotalRounds { get; set; }
        public string CountryId { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public bool HasLevels { get; set; }
        public string HigherCompId { get; set; }
        public string LowerCompId { get; set; }

        public int LeagueLevel { get; set; }

    }
}