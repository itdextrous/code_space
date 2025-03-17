using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WTP.Public.Code
{
    public class RequireSecureConnectionAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            var remoteIpAddress = filterContext.HttpContext.Connection.RemoteIpAddress;
            if (IPAddress.IsLoopback(remoteIpAddress) ||
                remoteIpAddress.Equals(IPAddress.IPv6Loopback) ||
                (remoteIpAddress.IsIPv6LinkLocal && !remoteIpAddress.IsIPv6SiteLocal))
            {
              
                return;
            }
        }

        protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
        {
            if (!string.Equals(filterContext.HttpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(filterContext.HttpContext.Request.Method, "HEAD", StringComparison.OrdinalIgnoreCase))
            {
                base.HandleNonHttpsRequest(filterContext);
            }
          
            string url = "https://" + filterContext.HttpContext.Request.Host + filterContext.HttpContext.Request.Path;

            filterContext.Result = new RedirectResult(url, permanent: true);
        }
    } 
}