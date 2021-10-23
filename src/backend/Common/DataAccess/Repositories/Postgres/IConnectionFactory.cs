using Npgsql;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public interface IConnectionFactory
	{
		NpgsqlConnection Create(string connectionString);
	}
}