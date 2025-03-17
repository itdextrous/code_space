using Microsoft.AspNetCore.Builder;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace NaviAir.Core.EventHandlers
{
	class MediaSecurityHandler : IComposer
    {
  
        public void Compose(IUmbracoBuilder builder)
        {
            
        }

        public void Compose(IApplicationBuilder app)
        {
            // Configure the route
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "MediaSecurityRoute",
                    pattern: "media/files/{folder}/{file?}",
                    defaults: new { controller = "NaviairMedia", action = "Index" });
            });
        }

    }
}
