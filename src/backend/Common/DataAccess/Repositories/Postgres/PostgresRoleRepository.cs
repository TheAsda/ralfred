using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.DataAccess.Repositories.InMemory.EntityConfiguration;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresRoleRepository : BasePostgresRepository, IRolesRepository
	{
		public PostgresRoleRepository(StorageConnection storageConnection, IConnectionFactory connectionFactory) : base(typeof(RoleMapper))
		{
			_storageConnection = storageConnection;
			_connectionFactory = connectionFactory;
		}

		private readonly StorageConnection _storageConnection;
		private readonly IConnectionFactory _connectionFactory;
	}
}