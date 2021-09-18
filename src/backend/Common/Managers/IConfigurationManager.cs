using Ralfred.Common.Types;


namespace Ralfred.Common.Managers
{
	public interface IConfigurationManager
	{
		Configuration? Get(string path);

		Configuration Apply(Configuration old, Configuration @new);
	}
}