using Microsoft.Extensions.Configuration;


namespace Common.IntegrationTests
{
	public static class ConfigurationHelper
	{
		private static readonly IConfiguration _configuration;

		public static string PostgresDatabaseConnectionString => _configuration.GetConnectionString("PostgresDatabase");

		static ConfigurationHelper() =>
			_configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false, true)
				.Build();
	}
}