using System;
using System.Linq;

using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.Mapping;

using Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfigurations;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public class ConnectionFactory : IConnectionFactory
	{
		private readonly MappingSchema _schema;

		private readonly StorageConnection _storageConnection;

		public ConnectionFactory(StorageConnection storageConnection)
		{
			_storageConnection = storageConnection;
			_schema = new MappingSchema();

			var mappers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
				.Where(x => typeof(IEntityTableConfiguration).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

			foreach (var mapper in mappers)
				((IEntityTableConfiguration)Activator.CreateInstance(mapper)!).Configure(_schema);
		}

		public DataConnection Create()
		{
			return new DataConnection(new LinqToDbConnectionOptions(
					new LinqToDbConnectionOptionsBuilder()
						.UseConnectionString(new PostgreSQLDataProvider(), _storageConnection.ConnectionString)
						.UseMappingSchema(_schema)
				)
			);
		}
	}
}