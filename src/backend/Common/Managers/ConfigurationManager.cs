using System.IO;
using System.Linq;
using System.Text;

using Ralfred.Common.Helpers;
using Ralfred.Common.Types;


namespace Ralfred.Common.Managers
{
	public class ConfigurationManager : IConfigurationManager
	{
		public ConfigurationManager(ISerializer serializer, IContentProvider contentProvider)
		{
			_serializer = serializer;
			_contentProvider = contentProvider;
		}

		public Configuration? Get(string path)
		{
			var content = _contentProvider.Get(path);

			if (content is null)
				return default;

			return _serializer.Deserialize<Configuration>(content);
		}

		public Configuration Merge(Configuration old, Configuration @new)
		{
			old.Engine = @new.Engine ?? old.Engine;
			old.EnableWebUi = @new.EnableWebUi ?? old.EnableWebUi;
			old.ConnectionString = @new.ConnectionString ?? old.ConnectionString;

			return old;
		}

		private readonly ISerializer _serializer;
		private readonly IContentProvider _contentProvider;
	}
}