namespace Ralfred.Common.DataAccess.Repositories.InMemory.Transactions
{
	public interface ITransactionScopeFactory
	{
		ITransactionScope BeginTransaction();
	}
}