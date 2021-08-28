using System;

using Microsoft.Extensions.DependencyInjection;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.Types;


namespace Ralfred.Common.DependencyInjection
{
	public static class StorageResolvingExtensions
	{
		public static void ConfigureStorageContext(this IServiceCollection services, string storageType)
		{
			var conversionSucceed = Enum.TryParse(typeof(StorageEngineType), storageType, true, out var storageEngine);

			if (!conversionSucceed)
				throw new Exception(); // TODO: concrete exception

			var targetType = storageEngine switch
			{
				StorageEngineType.InMemory => typeof(InMemoryStorageContext<>),
				StorageEngineType.Postgres => typeof(PostgreStorageContext<>),
				StorageEngineType.Mongo    => typeof(MongoStorageContext<>),
				StorageEngineType.Redis    => typeof(RedisStorageContext<>),

				// TODO: concrete exception
				_ => throw new ArgumentOutOfRangeException()
			};

			services.AddSingleton(typeof(IStorageContext<>), targetType);
		}
	}
}