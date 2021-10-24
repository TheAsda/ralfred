using Npgsql;

using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class ConnectionFactory : IConnectionFactory
	{
		public ConnectionFactory(StorageConnection storageConnection)
		{
			_storageConnection = storageConnection;
		}

		public NpgsqlConnection Create()
		{
			return new NpgsqlConnection(_storageConnection.ConnectionString);
		}

		private readonly StorageConnection _storageConnection;
	}
}