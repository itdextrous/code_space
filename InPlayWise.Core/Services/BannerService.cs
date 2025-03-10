using InPlayWise.Common.DTO;
using InPlayWise.Core.InMemoryServices;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _topMsgRepo;
        private readonly MatchInMemoryService _inMemory;
        private readonly IHttpContextService _httpContext;

        public BannerService(IBannerRepository topMsgRepo, MatchInMemoryService inMemory, IHttpContextService context)
        {
            _topMsgRepo = topMsgRepo;
            _inMemory = inMemory;
            _httpContext = context;
        }
        public async Task<Result<bool>> AddTopMessage(TopMessageDto message)
        {
            try
            {
                TopMessageEntity msg = new TopMessageEntity()
                {
                    Id = Guid.NewGuid(),
                    Message = message.Message,
                    Url = message.Url,
                };
                bool added = await _topMsgRepo.AddTopMessage(msg);
                return added ? Result<bool>.Success(item: true) : Result<bool>.InternalServerError("Failed to Add Message");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<bool>.InternalServerError();
            }
        }

        public async Task<Result<bool>> DeleteTopMessage(string id)
        {
            try
            {

                bool deleted = await _topMsgRepo.DeleteTopMessage(Guid.Parse(id));
                return deleted ? Result<bool>.Success(item: true) : Result<bool>.InternalServerError("Failed to Delete Message");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<bool>.InternalServerError();
            }
        }

        public async Task<Result<bool>> EditTopMessage(TopMessageDto message, string id)
        {
            try
            {
                TopMessageEntity msg = new TopMessageEntity()
                {
                    Id = Guid.Parse(id),
                    Message = message.Message,
                    Url = message.Url,
                };
                bool updated = await _topMsgRepo.UpdateTopMessage(msg);
                return updated ? Result<bool>.Success(item: true) : Result<bool>.InternalServerError("Failed to Update Message");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<bool>.InternalServerError();
            }
        }

        public async Task<Result<List<UpcomingMatch>>> GetFavouriteMatches()
        {
            try
            {
                string userId = _httpContext.GetUserId();
                List<string> favouriteTeamIds = await _topMsgRepo.GetFavouriteTeamsId(userId);

                List<UpcomingMatch> matches = _inMemory.GetUpcomingMatches()
                    .Where(match => favouriteTeamIds.Contains(match.HomeTeamId) || favouriteTeamIds.Contains(match.AwayTeamId))
                    .ToList();
                return Result<List<UpcomingMatch>>.Success(item:matches);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<List<UpcomingMatch>>.InternalServerError();
            }
        }

        public async Task<Result<List<TopMessageDto>>> GetTopMessage(string id)
        {
            try
            {

                if (string.IsNullOrEmpty(id))
                {
                    return Result<List<TopMessageDto>>.Success(item: (await _topMsgRepo.GetTopMessage(new Guid()))
                        .Select(msg => mapTopMsgEntityToDto(msg)).ToList());
                }
                Guid msgId = Guid.Parse(id);
                List<TopMessageEntity> msg = await _topMsgRepo.GetTopMessage(msgId);
                if (msg.Count == 0) return Result<List<TopMessageDto>>.NotFound();
                return Result<List<TopMessageDto>>.Success(item: new List<TopMessageDto>() { mapTopMsgEntityToDto(msg[0]) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<List<TopMessageDto>>.InternalServerError();
            }
        }

        private TopMessageDto mapTopMsgEntityToDto(TopMessageEntity msg)
        {
            return new TopMessageDto()
            {
                Id = msg.Id,
                Message = msg.Message,
                Url = msg.Url,
            };
        }
    }
}
