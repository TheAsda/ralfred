using System;
using System.Collections.Generic;
using System.Linq;

using EnsureArg;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.Exceptions;


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

		public Guid Create(Account account)
		{
			Ensure.Arg(account).IsNotNull();

			if (account.Id == Guid.Empty)
			{
				account.Id = Guid.NewGuid();
			}

			_storage.Add(account);

			return account.Id;
		}

		public void Delete(Guid accountId)
		{
			var index = _storage.FindIndex(x => x.Id == accountId);

			if (index == -1)
			{
				throw new NotFoundException($"Cannot find account with id {accountId}");
			}

			_storage.RemoveAt(index);
		}

		public Account GetByName(string accountName)
		{
			Ensure.Arg(accountName).IsNotNullOrWhiteSpace();

			return _storage.Single(x => x.Name != null && x.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase));
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

		public IEnumerable<Account> List()
		{
			return _storage;
		}

		private readonly List<Account> _storage;
	}
}