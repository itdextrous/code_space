//using Hangfire.Dashboard;
//using NaviAir.Core.Config;
//using Umbraco.Cms.Core.Security;
//using Umbraco.Cms.Core.Services;

//namespace Naviair.Web.Jobs
//{
//    public class UsernameWhitelistAuthorizationFilter : IDashboardAuthorizationFilter
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IUserService _userService;
//        public UsernameWhitelistAuthorizationFilter(IHttpContextAccessor httpContextAccessor, IUserService userService)
//        {
//            _httpContextAccessor = httpContextAccessor;
//            _userService = userService;
//        }

//        public bool Authorize(DashboardContext context)
//        {
//            var httpContext = _httpContextAccessor.HttpContext;
//            var services = httpContext.RequestServices;
//            var backOfficeSecurity = services.GetRequiredService<IBackOfficeSecurity>();
//            var currentMember = backOfficeSecurity.CurrentUser;
//            if (currentMember == null)
//            {
//                return false;
//            }
//            var currentUser = _userService.GetByUsername(currentMember.Name);
//            if (currentUser == null)
//            {
//                return false;
//            }
//            var hangfireAllowedUsers = AppSettings.HangfireAllowedUsers.Split(';');
//            return hangfireAllowedUsers.Any(hau => hau == currentUser.Name);
//        }
//    }
//}
