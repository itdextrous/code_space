using NaviAir.Core.Section;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Naviair.Core.Comparer
{
	public class NaviAirSectionComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Sections().Append<NaviAirSectionApplication>();
		
		}
	}
}
