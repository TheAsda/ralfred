using System.IO;


namespace Ralfred.Common.Helpers
{
	public class ContentProvider : IContentProvider
	{
		public string? Get(string path)
		{
			return !File.Exists(path) ? null : File.ReadAllText(path);
		}
	}
}