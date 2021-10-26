using System;

using AutoFixture;

using FluentAssertions;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.InMemory;


namespace Common.IntegrationTests.DataAccess.Repositories.InMemory
{
	[TestFixture]
	[Category("Integration")]
	public class InMemoryAccountRepositoryTests
	{
		[SetUp]
		public void Setup()
		{
			_target = new InMemoryAccountRepository();
		}

		[Test]
		public void ExistsTest()
		{
			// arrange
			var account = CreateAccount();

			var id = _target.Create(account);

			// act
			var result = _target.Exists(account.Name!);

			// assert
			result.Should().BeTrue();
			id.Should().NotBe(Guid.Empty);
		}

		[Test]
		public void Exists_ForNotExistingAccountTest()
		{
			// arrange
			var accountName = _fixture.Create<string>();

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

			var id = _target.Create(account);

			// act
			var result = _target.GetByName(account.Name!);

			// assert
			result.Should().NotBeNull();
			id.Should().NotBe(Guid.Empty);
		}

		[Test]
		public void UpdateTest()
		{
			// arrange
			var account = CreateAccount();

			var id = _target.Create(account);

			account.TokenHash = _fixture.Create<string>();

			// act
			_target.Update(account);

			// assert
			var updated = _target.GetByName(account.Name!);

			updated.Should().NotBeNull();
			updated.Should().BeEquivalentTo(account, e => e.Excluding(x => x.RoleIds));
			id.Should().NotBe(Guid.Empty);
		}

		private Account CreateAccount()
		{
			var account = _fixture.Build<Account>()
				.With(x => x.Name, _fixture.Create<string>())
				.With(x => x.TokenHash, _fixture.Create<string>())
				.With(x => x.CertificateThumbprint, _fixture.Create<string>())
				.Without(x => x.RoleIds)
				.Create();

			return account;
		}

		private readonly IFixture _fixture = new Fixture();

		private InMemoryAccountRepository _target;
	}
}