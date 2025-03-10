using System;
using Workfacta.Shared.Providers;

namespace SAASCLOUDAPP.API.Providers
{
    public class DateProvider : IDateProvider
    {
        private readonly IDateProviderFactory _factory;

        public DateProvider(IDateProviderFactory factory)
        {
            _factory = factory;
        }

        public DateTime UtcNow => _factory.Build().UtcNow;

        public DateTimeOffset OffsetUtcNow => _factory.Build().OffsetUtcNow;
    }
}