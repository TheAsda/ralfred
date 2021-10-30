using LinqToDB.Data;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public interface IConnectionFactory
	{
		DataConnection Create();
	}
}