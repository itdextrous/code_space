using InPlayWise.Common.Enums;
using InPlayWise.Data.Entities.FeaturesCountEntities;
using InPlayWise.Data.Entities.MembershipEntities;
using Microsoft.AspNetCore.Identity;

namespace InPlayWise.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool ThemeIsDark { get; set; } = true;
        public Subscription Subscription { get; set; }
        public CountryEnum Country { get; set; }
        public Language Language { get; set; }
        public List<FavouriteTeams> FavouriteTeams { get; set; }
        public List<FavouriteCompetitions> FavouriteCompetition { get; set; }
        public List<MatchAlert> MatchAlerts { get; set; }
        public UserProfile Profile { get; set; }
        public List<LiveInsightsPerGame> LiveInsightsPerGamesCounter { get; set; }
        public List<LeagueStatsCount> LeagueStatsCounter { get; set; }
        public ShockCounter ShockCounter { get; set; }
        public UserQuota UserQuota { get; set; }
        public List<PredictionCounter> PredictionCounter { get; set; }
        public List<CleverLabelsCounter> CleverLabelsCounter { get; set; }
        public bool EmailAlerts { get; set; }
        public bool DesktopAlerts { get; set; }
        public bool GoogleOauth { get; set; }
        public bool IsTrialAvailed { get; set; }
        public bool TrialActive { get; set; }
        public string ProductId { get; set; }

        public string ProfilePic { get; set; } = "";

        public string FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;


        //public bool BasicActivated { get; set; }
        //public bool PasswordProvided { get; set; }
        //public bool GoogleLogin { get; set; }
        //public bool MagicLinkSignup { get; set; }

    }
}



