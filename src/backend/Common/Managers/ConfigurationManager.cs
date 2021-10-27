using Ralfred.Common.Helpers;
using Ralfred.Common.Helpers.Serialization;
using Ralfred.Common.Types;


namespace Ralfred.Common.Managers
{
	public class ConfigurationManager : IConfigurationManager
	{
		public ConfigurationManager(ISerializer serializer, IContentProvider contentProvider, ICryptoService cryptoService)
		{
			_serializer = serializer;
			_contentProvider = contentProvider;
			_cryptoService = cryptoService;
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
			old.DefaultFormat = @new.DefaultFormat ?? old.DefaultFormat;
			old.ConnectionString = @new.ConnectionString ?? old.ConnectionString;

			return old;
		}

		public Configuration GetDefaultConfiguration()
		{
			return new Configuration
			{
				Engine = StorageEngineType.InMemory,
				DefaultFormat = FormatType.Json,
				RootToken = _cryptoService.GenerateKey(),
				EnableWebUi = true
			};
		}

		public void Save(string path, Configuration configuration)
		{
			var content = _serializer.Serialize(configuration);
			_contentProvider.Save(path, content!);
		}

		private readonly ISerializer _serializer;
		private readonly IContentProvider _contentProvider;
		private readonly ICryptoService _cryptoService;
	}
}