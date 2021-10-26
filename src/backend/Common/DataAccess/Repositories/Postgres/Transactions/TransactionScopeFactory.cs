namespace Ralfred.Common.DataAccess.Repositories.InMemory.Transactions
{
	public class TransactionScopeFactory : ITransactionScopeFactory
	{
		public ITransactionScope BeginTransaction()
		{
			return new SqlTransactionScope();
		}
	}
}