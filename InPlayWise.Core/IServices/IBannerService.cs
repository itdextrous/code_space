using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWise.Core.IServices
{
    public interface IBannerService
    {
        Task<Result<List<TopMessageDto>>> GetTopMessage(string id);
        Task<Result<bool>> AddTopMessage(TopMessageDto message);
        Task<Result<bool>> EditTopMessage(TopMessageDto message, string id);
        Task<Result<bool>> DeleteTopMessage(string id);

        Task<Result<List<UpcomingMatch>>> GetFavouriteMatches();

    }
}
