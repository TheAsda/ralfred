using System;
using System.Linq;
using System.Reflection;

using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

using Ralfred.Common.Types;


namespace Common.IntegrationTests
{
	public class Setup
	{
		public static IServiceProvider CreateServices()
		{
			var referencedAssembly = Assembly.Load("Ralfred.Common");

			return new ServiceCollection()
				.AddFluentMigratorCore()
				.ConfigureRunner(rb =>
				{
					rb.AddPostgres()
						.WithGlobalConnectionString(ConfigurationHelper.PostgresDatabaseConnectionString)
						.ScanIn(referencedAssembly).For.Migrations();
				})
				.BuildServiceProvider(false);
		}
	}
}