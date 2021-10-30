using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public class PostgresRoleRepository : IRolesRepository
	{
		private readonly IConnectionFactory _connectionFactory;

		private readonly StorageConnection _storageConnection;

		public PostgresRoleRepository(StorageConnection storageConnection, IConnectionFactory connectionFactory)
		{
			_storageConnection = storageConnection;
			_connectionFactory = connectionFactory;
		}
	}
}