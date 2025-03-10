using InPlayWise.Common.DTO;
using InPlayWise.Common.Enums;
using InPlayWise.Data.Entities.MembershipEntities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CountryEnum = InPlayWise.Common.Enums.CountryEnum;

namespace InPlayWise.Core.IServices
{
    public interface IProfileService
    {
        Task<Result<bool>> ThemeDark();
        Task<Result<bool>> SetDarkTheme();
        Task<Result<bool>> SetLightTheme();
        Task<Result<string>> GetUserName();
        Task<Result<string>> GetUserEmail();
        Task<Result<bool>> AddFavouriteTeams(List<string> teamIdList);
        Task<Result<bool>> AddFavouriteCompetition(List<string> competitionIdList);
        Task<Result<List<Competition>>> GetFavouriteCompetitions();
        Task<Result<List<Team>>> GetFavouriteTeams();
        Task<Result<CountryEnum>> GetUserCountry();
        Task<Result<Language>> GetUserLanguage();
        Task<Result<bool>> SetCountry(CountryEnum country);
        Task<Result<bool>> SetLanguage();

        Task<Result<UserQuota>> GetUserQuota();


        Task<Result<SetAlertsDto>> GetUserAlerts();
        Task<Result<bool>> SetUserAlerts(SetAlertsDto dto);


        Task<Result<bool>> RemoveFavouriteCompetitions(List<string> competitionIds);
        Task<Result<bool>> RemoveFavouriteTeams(List<string> teamIds);
        Task<Result<bool>> IsTrialAvailed();

        Task<Result<CountryEnum>> GetCountry();

        Task<Result<bool>> SetProfilePic(IFormFile img);
        Task<FileStreamResult> GetProfilePic();
        Task<Result<bool>> DeleteProfilePic();
        Task<Result<bool>> SetFullNameAsync(string firstName, string lastName);
        Task<Result<FullName>> GetFullNameAsync();
        //Task<Result<bool>> SetCountry(int countryCode);


    }
}
