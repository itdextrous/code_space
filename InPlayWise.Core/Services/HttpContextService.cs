using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Core.Services
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<HttpContextService> _logger;

        public HttpContextService(IHttpContextAccessor httpContext, ILogger<HttpContextService> logger)
        {
            _httpContext = httpContext;
            _logger = logger;
        }
        public string GetProductId()
        {
            try
            {

                // Get the productIdClaim ID claim
                var productIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ProductId");

                return productIdClaim?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user ID");
                return null;
            }
        }

        public string GetUserId()
        {
            try
            {
                if (_httpContext?.HttpContext?.User == null)
                {
                    return null;
                }
                //foreach (var claim in _httpContext.HttpContext.User.Claims)
                //{
                //    Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
                //}

                var userIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);


                return userIdClaim?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user ID");
                return null;
            }

        }
    }
}
