using System.Transactions;


namespace Ralfred.Common.DataAccess.Helpers
{
	public sealed class DatabaseTransactionScope : IDatabaseTransactionScope
	{
		public DatabaseTransactionScope()
		{
			_transactionScope = new TransactionScope(TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = TransactionManager.DefaultTimeout,
					IsolationLevel = IsolationLevel.ReadCommitted
				}, TransactionScopeAsyncFlowOption.Enabled);
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