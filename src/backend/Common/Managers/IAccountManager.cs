using System;
using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.Managers
{
	public interface IAccountManager
	{
		void CreateTokenAccount(string token);

		void DeleteAccount(Guid accountId);

		IEnumerable<Account> GetAccounts();
	}
}