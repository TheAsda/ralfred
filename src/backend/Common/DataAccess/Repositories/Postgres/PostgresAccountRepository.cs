using System;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresAccountRepository : BasePostgresRepository, IAccountRepository
	{
		public PostgresAccountRepository(StorageConnection storageConnection) : base(typeof(AccountMapper))
		{
			_storageConnection = storageConnection;
		}

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

		private readonly StorageConnection _storageConnection;
	}
}