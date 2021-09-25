using System;


namespace Ralfred.Common.DataAccess.Helpers
{
	public interface IDatabaseTransactionScope : IDisposable
	{
		void Commit();
	}
}