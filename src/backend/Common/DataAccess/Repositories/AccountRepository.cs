using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public sealed class AccountRepository : IAccountRepository
	{
		public AccountRepository(IStorageContext<Account> storageContext)
		{
			_storageContext = storageContext;
		}

		private readonly IStorageContext<Account> _storageContext;
	}
}