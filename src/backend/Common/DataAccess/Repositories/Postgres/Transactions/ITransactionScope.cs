using System;


namespace Ralfred.Common.DataAccess.Repositories.Postgres.Transactions
{
	public interface ITransactionScope : IDisposable
	{
		void Commit();
	}
}