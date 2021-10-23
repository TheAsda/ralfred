using System;

using Microsoft.Extensions.DependencyInjection;

using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.DataAccess.Repositories.InMemory;
using Ralfred.Common.Helpers.Serialization;
using Ralfred.Common.Types;


namespace Ralfred.Common.DependencyInjection
{
	public static class StorageResolvingExtensions
	{
		public static void ConfigureRepositoryContext(this IServiceCollection services, StorageEngineType storageEngine)
		{
			var targetType = storageEngine switch
			{
				StorageEngineType.InMemory => typeof(InMemoryRepositoryContext),
				StorageEngineType.Postgres => typeof(PostgreRepositoryContext),

				_ => throw new ArgumentOutOfRangeException()
			};

			services.AddSingleton<InMemoryAccountRepository>();
			services.AddSingleton<InMemoryGroupRepository>();
			services.AddSingleton<InMemorySecretsRepository>();
			services.AddSingleton<InMemoryRolesRepository>();

			services.AddSingleton(typeof(IRepositoryContext), targetType);
		}

		public delegate ISerializer? SerializerResolver(FormatType? format);
	}
}