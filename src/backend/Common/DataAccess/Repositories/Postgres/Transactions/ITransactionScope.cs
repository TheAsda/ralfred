using System;


namespace Ralfred.Common.DataAccess.Repositories.InMemory.Transactions
{
	public interface ITransactionScope : IDisposable
	{
		void Commit();
	}
}