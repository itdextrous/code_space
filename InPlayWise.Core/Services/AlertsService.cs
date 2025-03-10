using InPlayWise.Common.DTO;
using InPlayWise.Core.Hubs;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.Wrappers;
using InPlayWiseCore.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InPlayWise.Core.Services
{
	public class AlertsService : IAlertsService
    {
        private readonly IAlertsRepository _alertsRepo;
        private readonly IHttpContextService _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailServices _emailService;
        private readonly ILogger<AlertsService> _logger;
        private readonly IHubContext<AlertsHub> _alertsHubContext;
        

        public AlertsService(IAlertsRepository alertsRepo, UserManager<ApplicationUser> userManager, IEmailServices emailService, ILogger<AlertsService> logger, IHubContext<AlertsHub> alertsHubContext, IHttpContextService context)
        {
            _alertsRepo = alertsRepo;
            //_httpContext = httpContext;
            _httpContext = context;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            _alertsHubContext = alertsHubContext;
        }

        public async Task<Result<bool>> SetMatchAlert(MatchAlertRequestDto mAl)
        {
            try
            {
                ApplicationUser user = await GetUser();
                if(user is null || mAl.AlertBeforeInMinutes < 2) {
                    return new Result<bool>(400, false, "Bad Request", false);
                }
                UpcomingMatch match = await _alertsRepo.GetMatch(mAl.MatchId);
                MatchAlert alert = new MatchAlert()
                {
                    Id = Guid.NewGuid(),
                    MatchId = mAl.MatchId,
                    UserId = user.Id,
                    AlertTime = mAl.MatchTime.AddMinutes(-mAl.AlertBeforeInMinutes)
                };
                await _alertsRepo.AddAlert(alert);
                return new Result<bool>(200, true, "Alert set successfully", true);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<bool>(500, false, "Internal Server Error", false);
            }
        }




        private async Task<ApplicationUser> GetUser()
        {
            try
            {
                //if (_httpContext is null)
                //    return null;
                //var userIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = _httpContext.GetUserId();
                return await _userManager.FindByIdAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> SendAllAlerts()
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                List<MatchAlert> allAlerts = await _alertsRepo.GetTrigerredAlerts();
                List<UpcomingMatch> matches = await _alertsRepo.GetUpcomingMatches();
                foreach (MatchAlert alert in allAlerts)
                {
                    UpcomingMatch match = FindUpcomingMatchById(alert.MatchId, matches);
                    ApplicationUser user = (await _userManager.FindByIdAsync(_httpContext.GetUserId()));
                    if (alert.EmailAlert)
                    {
                        bool sent = await SendAlertEmail(match, user);
                        if (!sent)
                        {
                            _logger.LogWarning($"Failed to send email to {user.Email}");
                        }
                        
                    }
                    if (alert.NotificationAlert)
                    {
                        bool sent = await SendAlertNotification(user, match);
                        if (!sent)
                        {
                            _logger.LogWarning($"Failed to send notification to {user.UserName}");
                        }
                    }
                }
                bool deleted = await _alertsRepo.DeleteMultipleAlerts(allAlerts);
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        

        public UpcomingMatch FindUpcomingMatchById(string matchId, List<UpcomingMatch> matches)
        {
            foreach(UpcomingMatch match in matches)
            {
                if (match.Id.Equals(matchId))
                    return match;
            }
            return null;
        }


        private async Task<bool> SendAlertEmail(UpcomingMatch match, ApplicationUser user)
        {
            try
            {
                string content = $"Hi, {user.UserName}, this message is to remind you about the game between {match.HomeTeam.Name} and {match.AwayTeam.Name}. Don't miss your chance and get ready to bet.";
                string email = user.Email;
                return await _emailService.SendEmail(email, "Match Reminder from InplayWise", content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private async Task<bool> SendAlertNotification(ApplicationUser user, UpcomingMatch match)
        {
            try
			{
                string content =
					$"Place your bets now";
                string header = $"{match.HomeTeam.Name} vs {match.AwayTeam.Name} is here";

                AlertDto alert = new AlertDto()
                {
                    Header = header,
                    Content = content
                } ;

                await _alertsHubContext.Clients.User(user.Id).SendAsync("ReceiveAlerts", alert);
                Console.WriteLine("Send notification");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}