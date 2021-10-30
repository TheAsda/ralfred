using System;
using System.Collections.Generic;

using DapperExtensions;
using DapperExtensions.Predicate;

using EnsureThat;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public class PostgresAccountRepository : BasePostgresRepository, IAccountRepository
	{
		public PostgresAccountRepository(IConnectionFactory connectionFactory) : base(typeof(AccountMapper))
		{
			_connectionFactory = connectionFactory;
		}

		public bool Exists(string accountName)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(accountName);

			using var connection = _connectionFactory.Create();
			connection.Open();

			return connection.Get<Account>(
				Predicates.Field<Account>(x => x.Name, Operator.Eq, accountName)
			) is not null;
		}

		public bool ExistsWithToken(string tokenHash)
		{
			EnsureArg.IsNotNullOrWhiteSpace(tokenHash);

			using var connection = _connectionFactory.Create();
			connection.Open();

			return connection.Get<Account>(
				Predicates.Field<Account>(x => x.TokenHash, Operator.Eq, tokenHash)
			) is not null;
		}

		public Guid Create(Account account)
		{
			if (string.IsNullOrEmpty(account.Name))
			{
				EnsureArg.IsNotNullOrWhiteSpace(account.TokenHash);
			}

			if (account.Id == Guid.Empty)
			{
				account.Id = Guid.NewGuid();
			}

			using var connection = _connectionFactory.Create();
			connection.Open();

			connection.Insert(account);

			return account.Id;
		}

		public void Delete(Guid accountId)
		{
			using var connection = _connectionFactory.Create();
			connection.Open();

			connection.Delete<Account>(Predicates.Field<Account>(x => x.Id, Operator.Eq, accountId));
		}

		public Account GetByName(string accountName)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(accountName);

			using var connection = _connectionFactory.Create();
			connection.Open();

			return connection.Get<Account>(
				Predicates.Field<Account>(x => x.Name, Operator.Eq, accountName)
			);
		}

		public Account Update(Account account)
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

		public IEnumerable<Account> List()
		{
			using var connection = _connectionFactory.Create();
			connection.Open();

			return connection.GetList<Account>();
		}

		private readonly IConnectionFactory _connectionFactory;
	}
}