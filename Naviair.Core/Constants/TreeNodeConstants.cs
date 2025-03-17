namespace NaviAir.Core.Constants
{
	public class TreeNodeConstants
    {
        public static readonly TreeNodeConstants Root = new TreeNodeConstants("-1");
        public static readonly TreeNodeConstants Content = new TreeNodeConstants("content");

        public static IEnumerable<TreeNodeConstants> Values
        {
            get
            {
                yield return Root;
                yield return Content;
            }
        }
        public string Name { get; }

        private TreeNodeConstants(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}