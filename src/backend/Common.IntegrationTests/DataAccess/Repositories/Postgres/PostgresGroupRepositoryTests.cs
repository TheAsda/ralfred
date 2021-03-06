using AutoFixture;

using FluentAssertions;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Postgres;
using Ralfred.Common.DataAccess.Repositories.Postgres.Transactions;
using Ralfred.Common.Exceptions;
using Ralfred.Common.Types;


namespace Common.IntegrationTests.DataAccess.Repositories.Postgres
{
	[TestFixture]
	[Category("Integration")]
	public class PostgresGroupRepositoryTests
	{
		[SetUp]
		public void Setup()
		{
			var storageConnection = new StorageConnection
			{
				ConnectionString = ConfigurationHelper.PostgresDatabaseConnectionString
			};

			_transactionScopeFactory = new TransactionScopeFactory();
			_target = new PostgresGroupRepository(new ConnectionFactory(storageConnection));

			_transaction = _transactionScopeFactory.BeginTransaction();
		}

		[TearDown]
		public void TearDown()
		{
			_transaction.Dispose();
		}

		private readonly IFixture _fixture = new Fixture();

		private ITransactionScope _transaction;
		private ITransactionScopeFactory _transactionScopeFactory;

		private PostgresGroupRepository _target;

		[Test]
		public void DeleteGroupTest()
		{
			// arrange
			var groupName = _fixture.Create<string>();
			var groupPath = _fixture.Create<string>();

			_target.CreateGroup(groupName, groupPath);

			// act
			var existing = _target.Get(groupName, groupPath);
			existing.Should().NotBeNull();

			_target.DeleteGroup(groupName, groupPath);

			// assert
			Assert.Throws<NotFoundException>(() => _target.Get(groupName, groupPath));
		}

		[Test]
		public void ExistsTest()
		{
			// arrange
			var groupName = _fixture.Create<string>();
			var groupPath = _fixture.Create<string>();

			_target.CreateGroup(groupName, groupPath);

			// act
			var result = _target.Exists(groupName, groupPath);

			// assert
			result.Should().BeTrue();
		}

		[Test]
		public void GetTest()
		{
			// arrange
			var groupName = _fixture.Create<string>();
			var groupPath = _fixture.Create<string>();

			_target.CreateGroup(groupName, groupPath);

			// act
			var result = _target.Get(groupName, groupPath);

			// assert
			result.Should().NotBeNull();

			result.Should().BeEquivalentTo(new Group {
				Name = groupName,
				Path = groupPath
			}, e => e.Excluding(x => x.Id));
		}
	}
}