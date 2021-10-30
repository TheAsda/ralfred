namespace Ralfred.Common.DataAccess.Repositories.Postgres.Transactions
{
	public interface ITransactionScopeFactory
	{
		ITransactionScope BeginTransaction();
	}
}