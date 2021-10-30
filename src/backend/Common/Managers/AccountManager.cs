using System;
using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.Helpers;


namespace Ralfred.Common.Managers
{
	public class AccountManager : IAccountManager
	{
		public AccountManager(IRepositoryContext repositoryContext, ICryptoService cryptoService)
		{
			_cryptoService = cryptoService;
			_accountRepository = repositoryContext.GetAccountRepository();
		}

		public void CreateTokenAccount(string token)
		{
			_accountRepository.Create(new Account
			{
				TokenHash = _cryptoService.GetHash(token)
			});
		}

		public void DeleteAccount(Guid accountId)
		{
			_accountRepository.Delete(accountId);
		}

		public IEnumerable<Account> GetAccounts()
		{
			return _accountRepository.List();
		}

		private readonly IAccountRepository _accountRepository;
		private readonly ICryptoService _cryptoService;
	}
}