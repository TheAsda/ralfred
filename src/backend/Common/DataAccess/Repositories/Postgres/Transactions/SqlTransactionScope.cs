using System.Transactions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory.Transactions
{
	public sealed class SqlTransactionScope : ITransactionScope
	{
		public SqlTransactionScope()
		{
			_transactionScope = new TransactionScope(TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = TransactionManager.DefaultTimeout,
					IsolationLevel = IsolationLevel.ReadCommitted
				},
				TransactionScopeAsyncFlowOption.Enabled);
		}

		public void Commit()
		{
			_transactionScope.Complete();
			_transactionScope.Dispose();
		}

		public void Dispose()
		{
			_transactionScope.Dispose();
		}

		private readonly TransactionScope _transactionScope;
	}
}