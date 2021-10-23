using System;
using System.Collections.Generic;
using System.Linq;

using EnsureArg;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class InMemoryAccountRepository : IAccountRepository
	{
		public InMemoryAccountRepository()
		{
			_storage = new List<Account>();
		}

		public bool Exists(string accountName)
		{
			Ensure.Arg(accountName).IsNotNullOrWhiteSpace();

			return _storage.Any(x => x.Name != null && x.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase));
		}

		public void Add(Account account)
		{
			Ensure.Arg(account).IsNotNull();

			_storage.Add(account);
		}

		public Account? GetByName(string accountName)
		{
			Ensure.Arg(accountName).IsNotNullOrWhiteSpace();

			return _storage.SingleOrDefault(x => x.Name != null && x.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase));
		}

		public Account? Update(Account account)
		{
			if (string.IsNullOrEmpty(account.Name))
			{
				Ensure.Arg(account.TokenHash).IsNotNull();
			}

			var index = _storage.FindIndex(x => x.Id == account.Id);

			if (index == -1)
			{
				return null;
			}

			_storage[index] = account;

			return account;
		}

		private readonly List<Account> _storage;
	}
}