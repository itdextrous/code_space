using InPlayWise.Common.Constants;
using InPlayWise.Common.DTO.SportsApiResponseModels;
using InPlayWise.Core.IServices;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InPlayWise.Core.Services
{
    public class BasicInfoServices : IBasicInfoServices
    {
        private readonly IConfiguration _configuration;
        private readonly string _user;
        private readonly string _secret;
        private readonly ILogger<BasicInfoServices> _logger;

        public BasicInfoServices(IConfiguration configuration, ILogger<BasicInfoServices> logger)
        {
            _configuration = configuration;
            _user = _configuration.GetSection("SportsApi:User").Value;
            _secret = _configuration.GetSection("SportsApi:Secret").Value;
            _logger = logger;
        }


        public async Task<List<ApiCategory>> Category()
        {
            try
            {
                JsonElement dt = await DataFetcher(ApiRoutes.Category);
                List<ApiCategory> categories = dt.Deserialize<List<ApiCategory>>();
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<ApiCategory>();
            }
        }

        public async Task<List<ApiCountry>> Country()
        {
            try
            {
                JsonElement dt = await DataFetcher(ApiRoutes.Country);
                List<ApiCountry> countries = dt.Deserialize<List<ApiCountry>>();
                return countries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<ApiCountry>();
            }
        }

        public async Task<List<ApiCompetition>> Competition(string competitionId = "")
        {
            try
            {
                int page = 1;
                List<ApiCompetition> allComps = new List<ApiCompetition>();
                while(true)
                {
					string param = string.IsNullOrEmpty(competitionId) ? $"page={page}" : $"uuid={competitionId}";
					JsonElement dt = await DataFetcher(ApiRoutes.Competition, param);
					List<ApiCompetition> comp = dt.Deserialize<List<ApiCompetition>>();
                    allComps.AddRange(comp);
                    if (comp.Count == 0 || comp.Count == 1) break;
                    page++;
				}
                return allComps;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<ApiCompetition>();
            }
        }

        public async Task<List<ApiTeam>> Team(string teamId = "")
        {
            try
            {
                int page = 1;
                List<ApiTeam> allTeams = new List<ApiTeam>();
                
                while (true)
                {
                    string param = string.IsNullOrEmpty(teamId) ? $"page={page}" : $"uuid={teamId}";
                    JsonElement dt = await DataFetcher(ApiRoutes.Team, param);
                    List<ApiTeam> teams = dt.Deserialize<List<ApiTeam>>();
                    allTeams.AddRange(teams);
                    if (teams.Count == 0 || teams.Count == 1) break;
                    page++;
                }
                return allTeams;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<ApiTeam>();
            }

        }


        //public async Task<Result<PlayerResponseDto>> Player(string uuid = "")
        //{
        //    return await GetResult<PlayerResponseDto>("player/with_stat/list", uuid ) ;
        //}

        //public async Task<Result<CoachResponseDto>> Coach(string uuid = "")
        //{
        //    return await GetResult<CoachResponseDto>("coach/list", uuid) ;
        //}

        //public async Task<Result<RefereeResponseDto>> Referee(string uuid = "")
        //{
        //    return await GetResult<RefereeResponseDto>("referee/list", uuid) ;
        //}

        //public async Task<Result<VenueResponseModel>> Venue(string uuid = "")
        //{
        //    return await GetResult<VenueResponseModel>("venue/list", uuid) ;
        //}

        public async Task<List<ApiSeason>> Season(string seasonid = "")
        {
            try
            {
                int page = 1;
                List<ApiSeason> allSeasons = new List<ApiSeason>();
                while (true)
                {
                    string param = string.IsNullOrEmpty(seasonid) ? $"page={page}" : $"uuid={seasonid}";
					JsonElement dt = await DataFetcher(ApiRoutes.Season, param);
					List<ApiSeason> seasons = dt.Deserialize<List<ApiSeason>>();
                    allSeasons.AddRange(seasons);
                    if (seasons.Count == 0 || seasons.Count == 1) break;
                    page++;
				}

                return allSeasons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<List<ApiStage>> Stage(string stageId = "")
        {
            try
            {
                JsonElement dt = await DataFetcher(ApiRoutes.Stage, string.IsNullOrEmpty(stageId) ? "" : $"uuid={stageId}");
                List<ApiStage> stages = dt.Deserialize<List<ApiStage>>();
                return stages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<ApiStage>();
            }
        }

        //public async Task<Result<DataUpdateResponseModel>> DataUpdate()
        //{
        //    return await GetResult<DataUpdateResponseModel>("data/update") ;
        //}


        private async Task<JsonElement> DataFetcher(string url, string paramters = "")
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
                    return failureResponse;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.LogInformation(url + '\n' + jsonResponse);
                    _logger.LogError(ex.ToString());
                    return failureResponse;
                }
                catch(Exception ex2)
                {
                    _logger.LogError("Error in deeper block" + ex2.ToString());
                    return failureResponse;
                }

            }
        }

    }
}
