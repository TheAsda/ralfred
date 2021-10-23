using System;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresAccountRepository : IAccountRepository
	{
		public bool Exists(string accountName)
		{
			throw new NotImplementedException();
		}

		public void Add(Account account)
		{
			throw new NotImplementedException();
		}

		public Account? GetByName(string accountName)
		{
			throw new NotImplementedException();
		}

		public Account? Update(Account account)
		{
			throw new NotImplementedException();
		}
	}
}