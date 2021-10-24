using Npgsql;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public class ConnectionFactory : IConnectionFactory
	{
		public NpgsqlConnection Create(string connectionString)
		{
			return new NpgsqlConnection(connectionString);
		}
	}
}