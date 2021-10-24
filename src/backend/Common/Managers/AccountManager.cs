using System;
using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.Managers
{
	public class AccountManager : IAccountManager
	{
		public AccountManager(IRepositoryContext repositoryContext)
		{
			_accountRepository = repositoryContext.GetAccountRepository();
		}

		public Account CreateTokenAccount(string token)
		{
			throw new NotImplementedException();
		}

		public void DeleteAccount(Guid accountId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Account> GetAccounts()
		{
			throw new NotImplementedException();
		}

		private readonly IAccountRepository _accountRepository;
	}
}