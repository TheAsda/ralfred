using DapperExtensions;
using DapperExtensions.Predicate;

using EnsureArg;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresAccountRepository : BasePostgresRepository, IAccountRepository
	{
		public PostgresAccountRepository(StorageConnection storageConnection, IConnectionFactory connectionFactory)
			: base(typeof(AccountMapper))
		{
			_storageConnection = storageConnection;
			_connectionFactory = connectionFactory;
		}

		public bool Exists(string accountName)
		{
			Ensure.Arg(accountName).IsNotNullOrWhiteSpace();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			return connection.Get<Account>(
				Predicates.Field<Account>(x => x.Name, Operator.Eq, accountName)
			) is null;
		}

		public void Add(Account account)
		{
			if (string.IsNullOrEmpty(account.Name))
			{
				Ensure.Arg(account.TokenHash).IsNotNull();
			}

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			connection.Insert(account);
		}

		public Account? GetByName(string accountName)
		{
			Ensure.Arg(accountName).IsNotNullOrWhiteSpace();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			return connection.Get<Account>(
				Predicates.Field<Account>(x => x.Name, Operator.Eq, accountName)
			);
		}

		public Account? Update(Account account)
		{
			if (string.IsNullOrEmpty(account.Name))
			{
				Ensure.Arg(account.TokenHash).IsNotNull();
			}

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);

			connection.Open();
			connection.Update(account);

			return account;
		}

		private readonly StorageConnection _storageConnection;
		private readonly IConnectionFactory _connectionFactory;
	}
}