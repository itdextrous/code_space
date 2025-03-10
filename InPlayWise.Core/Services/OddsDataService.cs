using InPlayWise.Common.DTO.FootballResponseModels.OddsDataResponseModels;
using InPlayWise.Core.IServices;
using InPlayWiseCommon.Wrappers;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace InPlayWise.Core.Services
{
    public class OddsDataService : IOddsDataService
    {

        private readonly IConfiguration _configuration;
        private readonly string _user;
        private readonly string _secret;
        private readonly string _baseUrl;

        public OddsDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _user = _configuration.GetSection("SportsApi:User").Value;
            _secret = _configuration.GetSection("SportsApi:Secret").Value;
            _baseUrl = _configuration.GetSection("SportsApi:BaseUrl").Value;
        }

        public async Task<Result<RealTimeOddsResponseDto>> GetRealTimeOdds()
        {
            try
            {
                string url = _baseUrl + "odds/live" + '?' + $"user={_user}&secret={_secret}";
                Console.WriteLine("Checkng breakpoint");
                string jsonData = await DataFetcher(url);
                return new Result<RealTimeOddsResponseDto>
                {
                    IsSuccess = true,
                    Items = jsonData is null ? null : new RealTimeOddsResponseDto()
                };
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<RealTimeOddsResponseDto>
                {
                    IsSuccess = true,
                    Items = null,
                    Message = "Internal server error",
                    StatusCode = 500
                };
            }
        }

        public async Task<Result<SingleMatchOddsResponseDto>> GetSingleMatchOdds(string matchId)
        {
            try
            {
                //return (await GetResult<SingleMatchOddsResponseDto>("odds/history", matchId));
                string url = _baseUrl + "odds/history" + '?' + $"user={_user}&secret={_secret}" + 
                    $"&uuid={matchId}";
               // string somestr = "some str";
                string jsonData = await DataFetcher(url);

                Console.WriteLine("work");
                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<SingleMatchOddsResponseDto>
                {
                    IsSuccess = false,
                    Message = "Internal server error",
                    StatusCode = 500
                };
            }
        }

        private string PrepareUrl(string AdditionalUrl, string uuid = "")
        {
            string url = _baseUrl + AdditionalUrl + '?' + $"user={ _user }&secret={_secret}";
            return string.IsNullOrWhiteSpace(uuid) ? url : url + $"&uuid={uuid}";
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

        private async Task<Result<T>> GetResult<T>(string additionalUrl, string uuid = "")
        {
            try
            {
                string url = PrepareUrl(additionalUrl, uuid);
                var res = await DataFetcher(url);
                var data = JsonSerializer.Deserialize<T>(res);
                var result = new Result<T>()
                {
                    Items = data,
                    IsSuccess = data is null ? false : true
                };
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var result = new Result<T>()
                {
                    IsSuccess = false
                };
                return result;
            }
        }




    }
}
