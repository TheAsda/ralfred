using DapperExtensions;
using DapperExtensions.Predicate;

using EnsureThat;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresAccountRepository : BasePostgresRepository, IAccountRepository
	{
		public PostgresAccountRepository(IConnectionFactory connectionFactory) : base(typeof(AccountMapper))
		{
			_connectionFactory = connectionFactory;
		}

		public bool Exists(string? accountName)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(accountName);

			using var connection = _connectionFactory.Create();
			connection.Open();

			return connection.Get<Account>(
				Predicates.Field<Account>(x => x.Name, Operator.Eq, accountName)
			) is not null;
		}

		public void Add(Account account)
		{
			if (string.IsNullOrEmpty(account.Name))
			{
				EnsureArg.IsNotNullOrWhiteSpace(account.TokenHash);
			}

			using var connection = _connectionFactory.Create();
			connection.Open();

			connection.Insert(account);
		}

		public Account? GetByName(string? accountName)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(accountName);

			using var connection = _connectionFactory.Create();
			connection.Open();

			return connection.Get<Account>(
				Predicates.Field<Account>(x => x.Name, Operator.Eq, accountName)
			);
		}

		public Account? Update(Account account)
		{
			if (string.IsNullOrEmpty(account.Name))
			{
				EnsureArg.IsNotNullOrWhiteSpace(account.TokenHash);
			}

			using var connection = _connectionFactory.Create();

			connection.Open();
			connection.Update(account);

			return account;
		}

		private readonly IConnectionFactory _connectionFactory;
	}
}