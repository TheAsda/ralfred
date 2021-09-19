using Ralfred.Common.Types;


namespace Ralfred.Common.Managers
{
	public interface IConfigurationManager
	{
		Configuration? Get(string path);

		Configuration Merge(Configuration old, Configuration @new);
	}
}