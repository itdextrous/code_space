using InPlayWise.Common.Constants;
using InPlayWise.Common.DTO.FootballResponseModels.BasicDataResponseModels;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.IServices;
using InPlayWise.Data.SportsEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InPlayWise.Core.Services
{
    public class BasicDataServices : IBasicDataServices
    {
        private readonly IConfiguration _configuration;

        private readonly string _user;
        private readonly string _secret;
        private readonly string _baseUrl;
        private readonly ILogger<BasicDataServices> _logger;
        private readonly HttpClient _httpclient;

        public BasicDataServices(IConfiguration configuration, ILogger<BasicDataServices> logger)
        {
            _configuration = configuration;
            _user = _configuration.GetSection("SportsApi:User").Value;
            _secret = _configuration.GetSection("SportsApi:Secret").Value;
            _baseUrl = _configuration.GetSection("SportsApi:BaseUrl").Value;
            _logger = logger;
            _httpclient = new HttpClient();
        }


        public async Task<List<ApiRecentMatch>> MatchRecent(string matchId = "", long timeStamp = 0)
        {
            try
            {
                List<ApiRecentMatch> res = new List<ApiRecentMatch>();
                if (timeStamp != 0)
                {
                    JsonElement dt = await DataFetcher(ApiRoutes.RecentMatch, $"time={timeStamp}");
                    List<ApiRecentMatch> matches = dt.Deserialize<List<ApiRecentMatch>>();
                    return matches;
                }
                else if (string.IsNullOrEmpty(matchId))
                {
                    int page = 1;
                    while (true)
                    {
                        JsonElement dt = await DataFetcher(ApiRoutes.RecentMatch, $"page={page}");
                        List<ApiRecentMatch> matches = dt.Deserialize<List<ApiRecentMatch>>();
                        if (matches.Count == 0) break;
                        res.AddRange(matches);
                        page++;
                    }
                    return res;
                }
                else
                {
                    JsonElement dtSingle = await DataFetcher(ApiRoutes.RecentMatch, $"uuid={matchId}");
                    List<ApiRecentMatch> matchSingle = dtSingle.Deserialize<List<ApiRecentMatch>>();
                    return matchSingle;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<ApiRecentMatch>();
            }
        }

        public async Task<List<ApiRecentMatch>> ScheduleAndResultsDateQuery(long tsp = 0)
        {
            try
            {
                List<ApiRecentMatch> matches = new List<ApiRecentMatch>();
                JsonElement dt = await DataFetcher(ApiRoutes.ScheduleAndResultsDateQuery, tsp == 0 ? "" : $"tsp={tsp}");
                matches = dt.Deserialize<List<ApiRecentMatch>>();
                return matches;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<ApiRecentMatch>();
            }
        }

        public async Task<List<RecentMatchResponseDto>> ScheduleAndResultsSeasonQuery(string seasonId = "")
        {
            try
            {
                List<RecentMatchResponseDto> matches = new List<RecentMatchResponseDto>();
                JsonElement dt = await DataFetcher(ApiRoutes.SeasonResult, $"uuid={seasonId}");
                matches = dt.Deserialize<List<RecentMatchResponseDto>>();
                return matches;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<RecentMatchResponseDto>();
            }
        }

        public async Task<List<LiveMatchModel>> RealTimeData()
        {
            try
            {
                JsonElement jsonData = await DataFetcher(ApiRoutes.RealTimeData, "");
                if (!jsonData.EnumerateArray().Any())
                    return new List<LiveMatchModel>();

                List<DataList> dlLi = new List<DataList>();
                foreach (var jsonMatch in jsonData.EnumerateArray())
                {
                    DataList dl = GetDlForMatch(jsonMatch);
                    dlLi.Add(dl);
                }
                return DataListToLiveMatches(dlLi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<LiveMatchModel>();
            }
        }

        public async Task<JsonElement> RealTimeDataTest()
        {
            try
            {
                JsonElement jsonData = await DataFetcher(ApiRoutes.RealTimeData, "");
                //if (!jsonData.EnumerateArray().Any())
                //    return new List<LiveMatchModel>();

                return jsonData;

                //List<DataList> dlLi = new List<DataList>();
                //foreach (var jsonMatch in jsonData.EnumerateArray())
                //{
                //    DataList dl = GetDlForMatch(jsonMatch);
                //    dlLi.Add(dl);
                //}
                //return DataListToLiveMatches(dlLi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                //return new List<LiveMatchModel>();
                throw;
            }
        }
        public async Task<JsonResult> MatchTimelines(string uuid = "")
        {
            return new JsonResult(await DataFetcher("match/trend/detail"));
        }
        public async Task<JsonResult> SingleMatchLineUp(string uuid = "")
        {
            return new JsonResult(await DataFetcher("match/lineup/detail"));
        }

        public async Task<JsonResult> MatchPlayerStatistics(string uuid = "")
        {
            return new JsonResult(await DataFetcher("match/player_stats/list"));
        }
        public async Task<JsonResult> MatchTeamStatistics(string uuid = "")
        {
            return new JsonResult(await DataFetcher("match/team_stats/list"));
        }

        public async Task<JsonResult> H2H(string uuid = "")
        {
            return new JsonResult(await DataFetcher("match/analysis"));
        }

        public async Task<JsonResult> SeasonStandings(string uuid = "")
        {
            return new JsonResult(await DataFetcher("season/recent/table/detail"));
        }

        public async Task<DataList> StatisticalData(string matchId = "")
        {
            try
            {
                JsonElement jsonData = await DataFetcher(ApiRoutes.HistoricalStats, $"uuid={matchId}");
                if (jsonData.ValueKind == JsonValueKind.Array)
                    return null;
                return GetDlForMatch(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<JsonResult> PlayerStatistics(string uuid = "")
        {
            return new JsonResult(await DataFetcher("match/player_stats/detail"));
        }

        public async Task<JsonResult> TeamStatistics(string uuid = "")
        {
            return new JsonResult(await DataFetcher("match/team_stats/detail"));
        }

        public async Task<JsonResult> HistoricalCompensation(string uuid = "")
        {
            return new JsonResult(await DataFetcher("compensation/list"));
        }
        public async Task<JsonResult> RealTimeStandings(string uuid = "")
        {
            return new JsonResult(await DataFetcher("table/live"));
        }
        public async Task<JsonResult> Delete(string uuid = "")
        {
            return new JsonResult(await DataFetcher("football/deleted"));
        }
        public async Task<JsonResult> Competition(string uuid = "")
        {
            return new JsonResult(await DataFetcher("competition/recent/list"));
        }

        public async Task<JsonResult> TeamLineup(string uuid = "")
        {
            return new JsonResult(await DataFetcher("team/squad"));
        }




        private async Task<JsonElement> DataFetcher(string url, string paramters)
        {
            string failureResult = "[]";
            JsonElement failureResponse = JsonDocument.Parse(failureResult).RootElement;
            string jsonResponse = "";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    url = url + $"user={_user}&secret={_secret}&" + paramters;
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonDocument document = JsonDocument.Parse(jsonResponse);
                    JsonElement root = document.RootElement;
                    bool resultExists = root.TryGetProperty("results", out JsonElement result);
                    if (resultExists)
                        return result;
                    _logger.LogError(jsonResponse);
                    return failureResponse;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.LogError($"url is {url} \n and data is {jsonResponse}");
                    _logger.LogError(ex.ToString());
                    return failureResponse;
                }
                catch(Exception ex2)
                {
                    _logger.LogError($"Exception in inner level {ex2}");
                    return failureResponse;
                }

            }
        }

        private DataList GetDlForMatch(JsonElement match)
        {
            try
            {
                DataList dl = match.Deserialize<DataList>();
                int count = 0;
                bool scoreExists = match.TryGetProperty("score", out JsonElement score);
                if (!scoreExists) return null;
                foreach (var scoreField in score.EnumerateArray())
                {
                    switch (count)
                    {
                        case 0:
                            break;
                        case 1:
                            dl.Status = scoreField.GetInt32();
                            break;
                        case 2:
                            dl.HomeScore = scoreField.Deserialize<List<int>>();
                            break;
                        case 3:
                            dl.AwayScore = scoreField.Deserialize<List<int>>();
                            break;
                        case 4:
                            dl.KickOffTimeStamp = scoreField.GetInt64();
                            break;
                    }
                    count++;
                }
                return dl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        private async Task<string> DataFetcher(string url)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
        }


        private List<LiveMatchModel> DataListToLiveMatches(List<DataList> dataLi)
        {
            try
            {
                List<LiveMatchModel> result = new List<LiveMatchModel>();

                foreach (DataList dl in dataLi)
                {
                    int homeScore = dl.HomeScore[0] + (dl.HomeScore[5] == 0 ? 0 : dl.HomeScore[5] - dl.HomeScore[0]) + dl.HomeScore[6];
                    int awayScore = dl.AwayScore[0] + (dl.AwayScore[5] == 0 ? 0 : dl.AwayScore[5] - dl.AwayScore[0]) + dl.AwayScore[6];

                    LiveMatchModel match = new LiveMatchModel()
                    {
                        MatchId = dl.MatchId,
                        MatchStatus = dl.Status,
                        HomeGoals = homeScore,
                        AwayGoals = awayScore,
                        HomeRed = dl.HomeScore[2],
                        AwayRed = dl.AwayScore[2],
                        HomeYellow = dl.HomeScore[3],
                        AwayYellow = dl.AwayScore[3],
                        HomeCorners = dl.HomeScore[4],
                        AwayCorners = dl.AwayScore[4],
                        HomeTeamHalfTimeScore = dl.HomeScore[1],
                        AwayTeamHalfTimeScore = dl.AwayScore[1],
                        HomeTeamOverTimeScore = dl.AwayScore[5],
                        AwayTeamOverTimeScore = dl.AwayScore[5],
                        HomeTeamPenaltyShootOutScore = dl.HomeScore[6],
                        AwayTeamPenaltyShootoutScore = dl.AwayScore[6]
                    };

                    foreach (Stat st in dl.Stats)
                    {
                        int type = st.Type;
                        int home = st.Home;
                        int away = st.Away;
                        switch (type)
                        {
                            case 8:
                                match.HomePenalties = home == 0 ? match.HomePenalties : home;
                                match.AwayPenalties = away == 0 ? match.AwayPenalties : away;
                                break;
                            case 17:
                                match.HomeOwnGoals = home == 0 ? match.HomeOwnGoals : home;
                                match.AwayOwnGoals = away == 0 ? match.AwayOwnGoals : away;
                                break;
                            case 21:
                                match.HomeShotsOnTarget = home == 0 ? match.HomeShotsOnTarget : home;
                                match.AwayShotsOnTarget = away == 0 ? match.AwayShotsOnTarget : away;
                                break;
                            case 22:
                                match.HomeShotsOffTarget = home == 0 ? match.HomeShotsOffTarget : home;
                                match.AwayShotsOffTarget = away == 0 ? match.AwayShotsOffTarget : away;
                                break;
                            case 23:
                                match.HomeAttacks = home == 0 ? match.HomeAttacks : home;
                                match.AwayAttacks = away == 0 ? match.AwayAttacks : away;
                                break;
                            case 24:
                                match.HomeDangerousAttacks = home == 0 ? match.HomeDangerousAttacks : home;
                                match.AwayDangerousAttacks = away == 0 ? match.AwayDangerousAttacks : away;
                                break;
                            case 25:
                                match.HomePossession = home == 0 ? match.HomePossession : home;
                                match.AwayPossession = away == 0 ? match.AwayPossession : away;
                                break;
                        }
                    }

                    if(dl.Stats.Count > 0)
                    {
                        match.StatsComplete = true;
                    }

                    foreach (Incident inc in dl.Incidents)
                    {
                        int type = inc.Type;
                        int position = inc.Position;
                        int time = inc.Time;

                        if (type == 1 || type == 8 || type == 17)
                        {
                            string player = string.IsNullOrEmpty(inc.PlayerName) ? "empt" : inc.PlayerName;
                            if (position == 1)
                            {
                                match.HomeGoalMinutes += time + ",";
                                match.HomeScorers += player + ",";
                            }
                            else
                            {
                                match.AwayGoalMinutes += time + ",";
                                match.AwayScorers += player + ",";
                            }
                        }
                        else if (type == 3)
                        {
                            string player = string.IsNullOrEmpty(inc.PlayerName) ? "empt" : inc.PlayerName;
                            if (position == 1)
                            {
                                match.HomeYellowMinutes += time + ",";
                                match.HomeYellowNames += player + ",";
                            }

                            else
                            {
                                match.AwayYellowMinutes += time + ",";
                                match.AwayYellowNames += player + ",";
                            }
                        }
                        else if (type == 4 || type == 15)
                        {
                            string player = string.IsNullOrEmpty(inc.PlayerName) ? "empt" : inc.PlayerName;
                            if (position == 1)
                            {
                                match.HomeRedMinutes += time + ",";
                                match.HomeRedNames += player + ",";
                            }
                            else
                            {
                                match.AwayRedMinutes += time + ","; 
                                match.AwayRedNames += player + ",";
                            }
                               
                        }
                        if (type == 8)
                        {
                            if (position == 1)
                                match.HomePenaltiesRecord += "g" + ",";
                            else
                                match.AwayPenaltiesRecord += "g" + ",";
                        }
                        else if (type == 16)
                        {
                            if (position == 1)
                                match.HomePenaltiesRecord += "x" + ",";
                            else
                                match.AwayPenaltiesRecord += "x" + ",";
                        }
                        else if (type ==9)
                        {
                            string player = string.IsNullOrEmpty(inc.InSubPlayer) ? "empt" : inc.InSubPlayer;
                            if (position == 1) 
                            {
                                  match.HomeSubstitutionMinutes += time + ",";
                                  match.HomeSubstituteNames += player + ",";
                            }
                            else
                            {
                                match.AwaySubstitutionMinutes += time + ",";
                                match.AwaySubstituteNames += player + ",";
                            }
                        }
                    }
                    match.HomeSubstitutionMinutes = match.HomeSubstitutionMinutes.TrimEnd(',');
                    match.AwaySubstitutionMinutes = match.AwaySubstitutionMinutes.TrimEnd(',');
                    match.HomeGoalMinutes = match.HomeGoalMinutes.TrimEnd(',');
                    match.AwayGoalMinutes = match.AwayGoalMinutes.TrimEnd(',');
                    match.HomeScorers = match.HomeScorers.TrimEnd(',');
                    match.AwayScorers = match.AwayScorers.TrimEnd(',');
                    match.HomeYellowMinutes = match.HomeYellowMinutes.TrimEnd(',');
                    match.AwayYellowMinutes = match.AwayYellowMinutes.TrimEnd(',');
                    match.HomeYellowNames = match.HomeYellowNames.TrimEnd(',');
                    match.AwayYellowNames = match.AwayYellowNames.TrimEnd(',');
                    match.HomeRedMinutes = match.HomeRedMinutes.TrimEnd(',');
                    match.AwayRedMinutes = match.AwayRedMinutes.TrimEnd(',');
                    match.HomeRedNames = match.HomeRedNames.TrimEnd(',');
                    match.AwayRedNames = match.AwayRedNames.TrimEnd(',');
                    match.HomeSubstituteNames = match.HomeSubstituteNames.TrimEnd(',');
                    match.AwaySubstituteNames = match.AwaySubstituteNames.TrimEnd(',');
                    match.HomePenaltiesRecord = match.HomePenaltiesRecord.TrimEnd(',');
                    match.AwayPenaltiesRecord = match.AwayPenaltiesRecord.TrimEnd(',');
                    match.CurrentKickoffTime = dl.KickOffTimeStamp;
                    match.OverTime = dl.Status == 5;
                    match.PenaltyShootOut = dl.Status == 7;
                    match.Ended = dl.Status == 8;
                    short tm = (short)((DateTimeOffset.UtcNow.ToUnixTimeSeconds() - dl.KickOffTimeStamp) / 60 + 1);
                    int status = match.MatchStatus;
                    match.MatchMinutes = status == 2 ? tm : status == 3 ? 45 : status == 4 ? 45 + tm : status == 5 ? 90 : 0;
                    result.Add(match);
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<LiveMatchModel>();
            }
        }

        public async Task<object?> GetIpAddress(string url)
        {
            var response = await _httpclient.GetAsync(url);

            // Ensure the request was successful (Status Code 2xx)
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a dynamic object (or a specific model if you have one)
            var data = JsonSerializer.Deserialize<dynamic>(responseBody);

            // Return the JSON data
            return data;
        }
    }
}


