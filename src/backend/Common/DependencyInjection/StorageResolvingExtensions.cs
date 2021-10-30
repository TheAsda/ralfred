using System;

using Microsoft.Extensions.DependencyInjection;

using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.DataAccess.Repositories.InMemory;
using Ralfred.Common.DataAccess.Repositories.Postgres;
using Ralfred.Common.Helpers.Serialization;
using Ralfred.Common.Types;


namespace Ralfred.Common.DependencyInjection
{
	public static class StorageResolvingExtensions
	{
		public delegate ISerializer? SerializerResolver(FormatType? format);

		public static void ConfigureRepositoryContext(this IServiceCollection services, Configuration configuration)
		{
			var storageEngine = configuration.Engine!.Value;

			if (storageEngine != StorageEngineType.InMemory)
				services.AddTransient(_ => new StorageConnection
				{
					ConnectionString = configuration.ConnectionString!
				});

			switch (storageEngine)
			{
				case StorageEngineType.InMemory:
				{
					services.AddSingleton<InMemoryAccountRepository>();
					services.AddSingleton<InMemoryGroupRepository>();
					services.AddSingleton<InMemorySecretRepository>();
					services.AddSingleton<InMemoryRoleRepository>();

					services.AddSingleton<IRepositoryContext, InMemoryRepositoryContext>();

					break;
				}

				case StorageEngineType.Postgres:
				{
					services.AddTransient<IConnectionFactory, ConnectionFactory>();

					services.AddTransient<PostgresAccountRepository>();
					services.AddTransient<PostgresGroupRepository>();
					services.AddTransient<PostgresSecretRepository>();
					services.AddTransient<PostgresRoleRepository>();

					services.AddSingleton<IRepositoryContext, PostgresRepositoryContext>();

					break;
				}

				default: throw new ArgumentOutOfRangeException();
			}
		}
	}
}