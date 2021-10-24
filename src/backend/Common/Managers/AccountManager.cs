using System;
using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.Managers
{
	public class AccountManager : IAccountManager
	{
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
	}
}