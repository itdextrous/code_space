using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using WTP.Public.Code;

namespace NaviAir.Core.EventHandlers
{
    public class Global : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            // Register global filters

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireSecureConnectionAttribute());
            });
        }
    }
}