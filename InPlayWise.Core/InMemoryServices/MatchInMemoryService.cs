using InPlayWise.Common.DTO;
using InPlayWise.Common.DTO.CachedDto;
using InPlayWise.Data.DTO;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Core.InMemoryServices
{
    public class MatchInMemoryService
	{
		private List<LiveMatchDto> _liveMatches = new List<LiveMatchDto>();
		private List<LiveMatchDto> _previousLiveMatches = new List<LiveMatchDto>();
		private List<Insights> _insights = new List<Insights>();
		//private List<Opportunity> _opportunities = new List<Opportunity>();
		private List<OpportunitiesResponseDto> _oppList = new List<OpportunitiesResponseDto>();
		private List<CleverLabelsDto> _cleverLabels = new List<CleverLabelsDto>();
		//private List<RecentMatchModel> _recentMatches = new List<RecentMatchModel>();
		private List<UpcomingMatch> _upcomingMatches = new List<UpcomingMatch>();
		private List<HistoricalStatsResponseMatch> _historicalStatistics = new List<HistoricalStatsResponseMatch>();
		private List<LeagueStats> _leagueStatsTopTen = new List<LeagueStats>();
		private List<LeagueStats> _leagueStatsAll = new List<LeagueStats>();
		private List<UserSession> _userSessions = new List<UserSession>();
		private List<UserSession> _alertsUsers = new List<UserSession>();
		private List<AdvertisementGetDto> _advertisement = new List<AdvertisementGetDto>();
		private Dictionary<string, List<RecentMatchModel>> _pastMatchesHash = new Dictionary<string, List<RecentMatchModel>>();
		private List<Team> _teams = new List<Team>();
		private List<Competition> _competitions = new List<Competition>();
		private List<LiveMatchFilterDto> _liveFilters = new List<LiveMatchFilterDto>();
		private Dictionary<string, UserCachedInfo> _userData = new Dictionary<string, UserCachedInfo>();

		public List<LiveMatchDto> GetAllLiveMatches()
		{
			return _liveMatches ;
		}

		public List<LiveMatchDto> GetPreviousLiveMatches()
		{
			return _previousLiveMatches;
		}

		public void SetPreviousLiveMatches(List<LiveMatchDto> matches)
		{
			_previousLiveMatches = new List<LiveMatchDto>(matches);
		}


		public List<Insights> GetAllInsights() {
			return _insights;
		}

		//public List<Opportunity> GetAllOpportunities()
		//{
		//	return _opportunities;
		//}

		//public void SetOpportunities(List<Opportunity> opportunities)
		//{
		//	_opportunities = opportunities;
		//}

		public void SetMatches(List<LiveMatchDto> matches)
		{
			_liveMatches = matches;
		}

		public void SetInsights(List<Insights> insights)
		{
			_insights = insights;
		}


		public List<OpportunitiesResponseDto> GetAllOpportunitiesDto()
		{
			return _oppList;
		}

		public void SetAllOpportunitiesDto(List<OpportunitiesResponseDto> opps)
		{
			_oppList = opps;
		}

		public List<CleverLabelsDto> GetCleverLabels()
		{
			return _cleverLabels;
		}



		public void SetCleverLabels(List<CleverLabelsDto> labels)
		{
			_cleverLabels = labels;
		}

		//public List<RecentMatchModel> GetRecentMatches()
		//{
		//	return _recentMatches;
		//}

		//public void SetRecentMatchModel(List<RecentMatchModel> recentMatches)
		//{
		//	_recentMatches = recentMatches;
		//}


		public List<UpcomingMatch> GetUpcomingMatches()
		{
			return _upcomingMatches;
		}

		public void SetUpcomingMatches(List<UpcomingMatch> upcomingMatches)
		{
			_upcomingMatches = upcomingMatches;
		}


		public void SetHistoricalStats(List<HistoricalStatsResponseMatch> historicalStats)
		{
			_historicalStatistics = historicalStats;
		}

		public List<HistoricalStatsResponseMatch> GetHistoricalStats()
		{
			return _historicalStatistics;
		}


		public List<LeagueStats> GetLeagueStatsTopTen()
		{
			return _leagueStatsTopTen;
		}

        /// <summary>
        /// Retrieves the list of user sessions from the in-memory storage.
        /// </summary>
        /// <returns>The list of user sessions.</returns>
        public List<UserSession> GetUserSessions()
        {
            return _userSessions;
        }

        public void SetUserSessions(List<UserSession>_userSessions, UserSession userSessions)
		{			
            _userSessions.Add(userSessions);
        }


		public void SetLeagueStatsTopTen(List<LeagueStats> stats)
		{
			_leagueStatsTopTen = stats;
		}

		public List<UserSession> GetAlertsUsers() { return _alertsUsers;}
		public void SetAlertsUsers(List<UserSession> users) { _alertsUsers = users;}


		public void SetAdvertisement(List<AdvertisementGetDto> ad)
		{
			_advertisement = ad;
		}

		public List<AdvertisementGetDto> GetAdvertisement()
		{
			return _advertisement;
		}


        public void SetLeagueStatsAll(List<LeagueStats> stats)
        {
           _leagueStatsAll = stats;
        }

        public List<LeagueStats> GetLeagueStatsAll()
        {
            return _leagueStatsAll;
        }

        public void SetPastMatchesHash(Dictionary<string, List<RecentMatchModel>> data)
        {
			_pastMatchesHash = data;
        }

        public Dictionary<string, List<RecentMatchModel>> GetPastMatchesHash()
        {
			return _pastMatchesHash;
        }

        public void AddPastMatchHash(RecentMatchModel match)
        {
			if (!match.Ended) 
			{
				return;
			}
            // HomeTeam
            if (_pastMatchesHash.ContainsKey(match.HomeTeamId))
            {
                if (!_pastMatchesHash[match.HomeTeamId].Any(existingMatch => existingMatch.MatchId == match.MatchId))
                {
                    _pastMatchesHash[match.HomeTeamId].Add(match);
                }
            }
            else
            {
                _pastMatchesHash[match.HomeTeamId] = new List<RecentMatchModel> { match };
            }

            // AwayTeam
            if (_pastMatchesHash.ContainsKey(match.AwayTeamId))
            {
                if (!_pastMatchesHash[match.AwayTeamId].Any(existingMatch => existingMatch.MatchId == match.MatchId))
                {
                    _pastMatchesHash[match.AwayTeamId].Add(match);
                }
            }
            else
            {
                _pastMatchesHash[match.AwayTeamId] = new List<RecentMatchModel> { match };
            }
        }

        public void SetTeams(List<Team> teams)
        {
            _teams = teams;
        }
        public List<Team> GetTeams()
        {
            return _teams;
        }

        public List<Competition> GetCompetitions()
        {
            return _competitions;
        }

        public void SetLiveFilters(List<LiveMatchFilterDto> liveFilters)
        {
			_liveFilters = liveFilters;
        }

        public List<LiveMatchFilterDto> GetLiveFilters()
        {
            return _liveFilters;
        }

        public void SetCompetitions(List<Competition> competitions)
        {
            _competitions = competitions;
        }

        public UserCachedInfo GetUserData(string userId)
        {
            return _userData.ContainsKey(userId) ? _userData[userId] : null;
        }

        public void SetUserData(string userId, UserCachedInfo data)
        {
			_userData[userId] = data;
        }


        //      public void ClearResources()
        //{
        //	_cleverLabels.Clear();
        //	_insights.Clear();
        //	_liveMatches.Clear();
        //	_oppList.Clear();
        //	//_opportunities.Clear();
        //	//_recentMatches.Clear();
        //	_upcomingMatches.Clear();
        //}

    }
}
