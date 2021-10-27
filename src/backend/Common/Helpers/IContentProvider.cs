using System;


namespace Ralfred.Common.Helpers
{
	public interface IContentProvider
	{
		public string? Get(string path);

		public void Save(string path, string content);
	}
}