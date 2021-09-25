namespace Ralfred.Common.DataAccess.Helpers
{
	public class TransactionFactory : ITransactionFactory
	{
		public IDatabaseTransactionScope BeginTransaction()
		{
			return new DatabaseTransactionScope();
		}
	}
}