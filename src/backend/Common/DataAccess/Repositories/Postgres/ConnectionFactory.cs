using Npgsql;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class ConnectionFactory : IConnectionFactory
	{
		public NpgsqlConnection Create(string connectionString)
		{
			return new NpgsqlConnection(connectionString);
		}
	}
}