using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AutoFixture;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Helpers;
using Ralfred.Common.DataAccess.Repositories;


namespace SecretsService.UnitTests.DataAccess.Repositories
{
	[TestFixture]
	public class SecretsRepositoryTests
	{
		[SetUp]
		public void Setup()
		{
			_fixture = new Fixture();

			_secretContext = new Mock<IStorageContext<Secret>>();
			_groupContext = new Mock<IStorageContext<Group>>();
			_transactionFactory = new Mock<ITransactionFactory>();

			_target = new SecretsRepository(_secretContext.Object, _groupContext.Object, _transactionFactory.Object);
		}

		[Test]
		public void GetGroupSecretsTest()
		{
			// arrange
			var group = _fixture.Create<Group>();
			var secret = _fixture.Build<Secret>().With(x => x.GroupId, group.Id).Create();

			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(group);
			_secretContext.Setup(x => x.List(It.IsAny<Expression<Func<Secret, bool>>>())).Returns(new List<Secret> { secret });

			// act
			var result = _target.GetGroupSecrets(group.Path, group.Name).ToList();

			// assert
			result.Should().HaveCount(1);
			result.Single().Should().BeEquivalentTo(secret);
		}

		[Test]
		public void UpdateGroupSecretsTest()
		{
			// arrange
			const string name = "test";
			const string path = "";

			var secret = _fixture.Create<Secret>();
			var group = _fixture.Build<Group>().With(x => x.Name, name).With(x => x.Path, path).Create();

			var mockedTransaction = new Mock<IDatabaseTransactionScope>();

			var secrets = new Dictionary<string, string>
			{
				{ "test", "test" },
				{ "test2", "test2" }
			};

			var files = new Dictionary<string, string>
			{
				{ "file", "file" }
			};

			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(group);

			_transactionFactory.Setup(x => x.BeginTransaction()).Returns(mockedTransaction.Object);
			mockedTransaction.Setup(x => x.Commit()).Verifiable();

			_secretContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Secret>>>())).Returns(secret);
			_secretContext.Setup(x => x.Update(It.IsAny<Secret>()));

			// act
			_target.UpdateGroupSecrets(name, path, secrets, files);

			// assert
			_groupContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);

			_transactionFactory.Verify(x => x.BeginTransaction(), Times.Once);
			mockedTransaction.Verify(x => x.Commit(), Times.Once);

			_secretContext.Verify(x => x.Update(It.IsAny<Secret>()), Times.Exactly(secrets.Keys.Count + files.Keys.Count));
			_secretContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Secret>>>()),
				Times.Exactly(secrets.Keys.Count + files.Keys.Count));
		}

		[Test]
		public void SetGroupSecretsTest()
		{
			// arrange
			const string name = "test";
			const string path = "";

			var mockedTransaction = new Mock<IDatabaseTransactionScope>();
			var group = _fixture.Build<Group>().With(x => x.Name, name).With(x => x.Path, path).Create();

			var secrets = new Dictionary<string, string>
			{
				{ "test", "test" },
				{ "test2", "test2" }
			};

			var files = new Dictionary<string, string>
			{
				{ "file", "file" }
			};

			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(group);

			_transactionFactory.Setup(x => x.BeginTransaction()).Returns(mockedTransaction.Object);
			mockedTransaction.Setup(x => x.Commit()).Verifiable();

			_secretContext.Setup(x => x.Add(It.IsAny<Secret>()));

			// act
			_target.SetGroupSecrets(name, path, secrets, files);

			// assert
			_groupContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);

			_transactionFactory.Verify(x => x.BeginTransaction(), Times.Once);
			mockedTransaction.Verify(x => x.Commit(), Times.Once);

			_secretContext.Verify(x => x.Add(It.IsAny<Secret>()), Times.Exactly(secrets.Keys.Count + files.Keys.Count));
		}

		[Test]
		public void DeleteGroupSecretsTest()
		{
			// arrange
			var group = _fixture.Create<Group>();
			var secrets = _fixture.Build<Secret>().With(x => x.GroupId, () => group.Id).CreateMany().ToList();

			var mockedTransaction = new Mock<IDatabaseTransactionScope>();

			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(group);
			_secretContext.Setup(x => x.Delete(It.IsAny<Expression<Func<Secret, bool>>>())).Verifiable();

			_transactionFactory.Setup(x => x.BeginTransaction()).Returns(mockedTransaction.Object);
			mockedTransaction.Setup(x => x.Commit()).Verifiable();

			// act
			var enumerable = secrets.ToList();
			_target.DeleteGroupSecrets(group.Name, group.Path, enumerable.Select(x => x.Name));

			// assert
			_groupContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);
			_secretContext.Verify(x => x.Delete(It.IsAny<Expression<Func<Secret, bool>>>()), Times.Exactly(secrets.Count));

			_transactionFactory.Verify(x => x.BeginTransaction(), Times.Once);
			mockedTransaction.Verify(x => x.Commit(), Times.Once);
		}

		private IFixture _fixture;

		private Mock<IStorageContext<Group>> _groupContext;
		private Mock<IStorageContext<Secret>> _secretContext;
		private Mock<ITransactionFactory> _transactionFactory;

		private ISecretsRepository _target;
	}
}