using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NaviAir.Core.Controller;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Extensions;

namespace Naviair.Core.Comparer
{
	public class NaviairAuthorizedApiComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.Configure<UmbracoPipelineOptions>(options =>
			{
				options.AddFilter(new UmbracoPipelineFilter(nameof(NaviairAuthorizedApiController))
				{
					Endpoints = app => app.UseEndpoints(endpoints =>
					{
						var globalSettings = app.ApplicationServices
							.GetRequiredService<IOptions<GlobalSettings>>().Value;
						var hostingEnvironment = app.ApplicationServices
							.GetRequiredService<IHostingEnvironment>();
						var backofficeArea = Constants.Web.Mvc.BackOfficePathSegment;

						var rootSegment = $"{globalSettings.GetUmbracoMvcArea(hostingEnvironment)}/{backofficeArea}";
						var areaName = "api";
						endpoints.MapUmbracoRoute<NaviairAuthorizedApiController>(rootSegment, areaName, areaName);
					})
				});
			});
		}
	}
}
