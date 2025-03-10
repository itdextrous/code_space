using System.Web;
using Workfacta.Shared.Providers;

namespace SAASCLOUDAPP.API.Providers
{
    public class ClientIpAddressAccessor : IClientIpAddressAccessor
    {
        public string GetIpAddress()
        {
            try
            {
                var serverVariables = HttpContext.Current?.Request?.ServerVariables;
                if (serverVariables == null) return null; // This may occur if we are not in a thread with a request in it.

                // If the request is forwarded, use this as the source IP.
                var ipAddress = serverVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipAddress))
                {
                    // Otherwise, use the remote address.
                    ipAddress = serverVariables["REMOTE_ADDR"];
                }

                return ipAddress;
            }
            catch
            {
                // If this ever fails, we simply return null.
                return null;
            }
        }
    }
}