using System;
using System.Web;
using Workfacta.Shared.Providers;

namespace SAASCLOUDAPP.API.Providers
{
    /// <summary>
    /// Cache on initial creation.
    /// Typically required when creating an instance that should be bound to a particular HTTP request but due to 
    /// thread switching ConfigureAwait(true) loses the current HTTP context.
    /// </summary>
    public class DateProviderWithHeaderOverrideFactory : IDateProviderFactory
    {
        public IDateProvider DateProvider { get; }

        public DateProviderWithHeaderOverrideFactory()
        {
            // Cache the headers.
            var currentContext = HttpContext.Current;

            DateProvider = new DateProviderWithHeaderOverride(() => currentContext);
        }
    }
}