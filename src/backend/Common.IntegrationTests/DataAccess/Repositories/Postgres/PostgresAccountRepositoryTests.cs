using System.Linq;

using AutoFixture;

using FluentAssertions;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.InMemory;
using Ralfred.Common.DataAccess.Repositories.InMemory.Transactions;
using Ralfred.Common.Types;


namespace Common.IntegrationTests.DataAccess.Repositories.Postgres
{
	[TestFixture]
	[Category("Integration")]
	public class PostgresAccountRepositoryTests
	{
		[SetUp]
		public void Setup()
		{
			var storageConnection = new StorageConnection
			{
				ConnectionString = ConfigurationHelper.PostgresDatabaseConnectionString
			};

			_transactionScopeFactory = new TransactionScopeFactory();
			_target = new PostgresAccountRepository(new ConnectionFactory(storageConnection));

			_transaction = _transactionScopeFactory.BeginTransaction();
		}

		[TearDown]
		public void TearDown()
		{
			_transaction.Dispose();
		}

		[Test]
		public void ExistsTest()
		{
			// arrange
			var account = CreateAccount();

			_target.Add(account);

			// act
			var result = _target.Exists(account.Name);

			// assert
			result.Should().BeTrue();
		}

		[Test]
		public void Exists_ForNotExistingAccountTest()
		{
			// arrange
			var accountName = CreateStringWithMaxLength(255);

			// act
			var result = _target.Exists(accountName);

			// assert
			result.Should().BeFalse();
		}

		[Test]
		public void GetByNameTest()
		{
			// arrange
			var account = CreateAccount();

			_target.Add(account);

			// act
			var result = _target.GetByName(account.Name);

			// assert
			result.Should().NotBeNull();
		}

		[Test]
		public void UpdateTest()
		{
			// arrange
			var account = CreateAccount();
			var tokenHashLength = _fixture.Create<Generator<int>>().First(x => x < 64);

			_target.Add(account);

			account.TokenHash = CreateStringWithMaxLength(tokenHashLength);

			// act
			_target.Update(account);

			// assert
			var updated = _target.GetByName(account.Name);

			updated.Should().NotBeNull();
			updated.Should().BeEquivalentTo(account, e => e.Excluding(x => x.RoleIds));
		}

		private string CreateStringWithMaxLength(int length)
		{
			return string.Join(string.Empty, _fixture.CreateMany<char>(length));
		}

		private Account CreateAccount()
		{
			var nameLength = _fixture.Create<Generator<int>>().First(x => x < 255);
			var tokenHashLength = _fixture.Create<Generator<int>>().First(x => x < 64);
			var certificateThumbprintLength = _fixture.Create<Generator<int>>().First(x => x < 40);

			var account = _fixture.Build<Account>()
				.With(x => x.Name, CreateStringWithMaxLength(nameLength))
				.With(x => x.TokenHash, CreateStringWithMaxLength(tokenHashLength))
				.With(x => x.CertificateThumbprint, CreateStringWithMaxLength(certificateThumbprintLength))
				.Without(x => x.RoleIds)
				.Create();

			return account;
		}

		private IFixture _fixture = new Fixture();

		private ITransactionScope _transaction;
		private ITransactionScopeFactory _transactionScopeFactory;

		private PostgresAccountRepository _target;
	}
}