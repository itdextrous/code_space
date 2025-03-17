using NaviAir.Core.Model;
using System.Globalization;

namespace NaviAir.Core.Comparer
{
	public class DanishComparer : IComparer<SimpleOrderByModel>
    {
        public int Compare(SimpleOrderByModel x, SimpleOrderByModel y)
        {
            var culture = new CultureInfo("da-DK");
            var comp = StringComparer.Create(culture, ignoreCase: true);
            return comp.Compare(x.Title, y.Title);
        }
    }
}