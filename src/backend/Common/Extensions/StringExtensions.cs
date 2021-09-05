using System.Linq;
using System.Text.RegularExpressions;


namespace Ralfred.Common.Extensions
{
	public static class StringExtensions
	{
		public static (string name, string path) DeconstructPath(this string fullPath)
		{
			const string separator = "/";

			var splitted = fullPath.Split(separator);

			var name = splitted.Last();
			var path = string.Join(separator, splitted.Take(splitted.Length - 1));

			return (name, path);
		}

		public static bool ValidatePath(this string path)
		{
			var pattern = new Regex(@"^[a-zA-Z0-9\-_]+(\/[a-zA-Z0-9\-_]+)*$");

			return pattern.IsMatch(path);
		}
	}
}