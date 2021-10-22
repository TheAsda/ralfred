using System;

using AutoFixture;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;


namespace SecretsService.UnitTests.DataAccess.Repositories
{
	[TestFixture]
		public class AccountRepositoryTests
	{
		[SetUp]
		public void Setup()
		{
			_storageContext = new Mock<IStorageContext<Account>>();

			_target = new AccountRepository(_storageContext.Object);
		}

		[Test]
		public void ExistsTest()
		{
			// arrange
			var accountName = _fixture.Create<string>();

			var account = _fixture.Build<Account>()
				.With(x => x.Name, accountName)
				.Create();

			_storageContext
				.Setup(x => x.Find(y => y.Name != null && y.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase)))
				.Returns(account);

			// act
			var result = _target.Exists(accountName);

			// assert
			result.Should().BeTrue();

			_storageContext
				.Verify(x => x.Find(y => y.Name != null && y.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase)), Times.Once);
		}

		[Test]
		public void AddTest()
		{
			// arrange
			var account = _fixture.Create<Account>();

			_storageContext.Setup(x => x.Add(account)).Verifiable();

			// act
			_target.Add(account);

			// assert
			_storageContext.Verify(x => x.Add(account), Times.Once);
		}

		[Test]
		public void GetByNameTest()
		{
			// arrange
			var accountName = _fixture.Create<string>();
			var account = _fixture.Create<Account>();

			_storageContext
				.Setup(x => x.Find(y => y.Name != null && y.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase)))
				.Returns(account);

			// act
			var result = _target.GetByName(accountName);

			// assert
			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(account);

			_storageContext
				.Verify(x => x.Find(y => y.Name != null && y.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase)), Times.Once);
		}

		[Test]
		public void UpdateTest()
		{
			// arrange
			var oldAccount = _fixture.Create<Account>();
			var newAccount = _fixture.Create<Account>();

			_storageContext.Setup(x => x.Update(oldAccount)).Returns(newAccount);

			// act
			var result = _target.Update(oldAccount);

			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(newAccount);

			_storageContext.Verify(x => x.Update(oldAccount), Times.Once);
		}

		[Test]
		public void Update_NoAccountNameTest()
		{
			// arrange
			var oldAccount = _fixture.Build<Account>().Without(x => x.Name).Create();
			var newAccount = _fixture.Create<Account>();

			_storageContext.Setup(x => x.Update(oldAccount)).Returns(newAccount);

			// act
			var result = _target.Update(oldAccount);

			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(newAccount);

			_storageContext.Verify(x => x.Update(oldAccount), Times.Once);
		}

		private IFixture _fixture = new Fixture();

		private Mock<IStorageContext<Account>> _storageContext;
		private AccountRepository _target;
	}
}