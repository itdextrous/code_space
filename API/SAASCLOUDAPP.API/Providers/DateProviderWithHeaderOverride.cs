using System;
using System.Web;
using System.Globalization;
using Workfacta.Shared.Providers;

namespace SAASCLOUDAPP.API.Providers
{
    internal class DateProviderWithHeaderOverride : IDateProvider
    {
        private const string DATE_HEADER_NAME = "x-date-override";
        private const string ISO_DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss.FFFFFFFK"; // Date format with a variable number of zeroes.

        private readonly Func<HttpContext> _httpContext;

        public DateProviderWithHeaderOverride()
            : this(() => HttpContext.Current)
        {
        }

        internal DateProviderWithHeaderOverride(Func<HttpContext> httpContext)
        {
            _httpContext = httpContext;
        }

        public DateTime UtcNow
        {
            get
            {
                var headerOffset = GetHeaderOffset(_httpContext());
                if (headerOffset == null) return DateTime.UtcNow;
                return DateTime.UtcNow - headerOffset.Value;
            }
        }

        public DateTimeOffset OffsetUtcNow
        {
            get
            {
                var headerOffset = GetHeaderOffset(_httpContext());
                if (headerOffset == null) return DateTimeOffset.UtcNow;
                return DateTimeOffset.UtcNow - headerOffset.Value;
            }
        }

        private static TimeSpan? GetHeaderOffset(HttpContext httpContext)
        {
            var dateHeader = GetDateFromHeader(httpContext);
            if (dateHeader == null) return null;
            return httpContext.Timestamp.ToUniversalTime() - dateHeader.Value;
        }

        private static DateTime? GetDateFromHeader(HttpContext httpContext)
        {
            var dateHeaderValue = httpContext.Request.Headers[DATE_HEADER_NAME];
            if (string.IsNullOrEmpty(dateHeaderValue)) return null;
            return DateTime.TryParseExact(dateHeaderValue, new string[] { "O", ISO_DATE_FORMAT }, null, DateTimeStyles.AdjustToUniversal, out var dateHeader) ? dateHeader : (DateTime?)null;
        }
    }
}