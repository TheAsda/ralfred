using Npgsql;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public interface IConnectionFactory
	{
		NpgsqlConnection Create(string connectionString);
	}
}