namespace Ralfred.Common.DataAccess.Helpers
{
	public interface ITransactionFactory
	{
		IDatabaseTransactionScope BeginTransaction();
	}
}