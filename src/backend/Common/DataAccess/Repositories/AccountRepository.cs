using System;

using EnsureArg;

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

		public bool Exists(string accountName)
		{
			Ensure.Arg(accountName).IsNotNullOrWhiteSpace();

			var account = _storageContext.Find(x => x.Name != null && x.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase));

			return account != null;
		}

		public void Add(Account account)
		{
			Ensure.Arg(account).IsNotNull();

			_storageContext.Add(account);
		}

		public Account? GetByName(string accountName)
		{
			Ensure.Arg(accountName).IsNotNullOrWhiteSpace();

			return _storageContext.Find(x => x.Name != null && x.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase));
		}

		public Account? Update(Account account)
		{
			if (string.IsNullOrEmpty(account.Name))
			{
				Ensure.Arg(account.TokenHash).IsNotNull();
			}

			return _storageContext.Update(account);
		}

		private readonly IStorageContext<Account> _storageContext;
	}
}