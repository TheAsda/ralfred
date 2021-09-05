using Ralfred.Common.Types;


namespace Ralfred.SecretsProvider.Helpers
{
	public interface IPathResolver
	{
		PathType Resolve(string path);
	}
}