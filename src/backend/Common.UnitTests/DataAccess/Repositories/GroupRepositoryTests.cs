using System;
using System.Collections.Generic;
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
	public class GroupRepositoryTests
	{
		[SetUp]
		public void Setup()
		{
			_fixture = new Fixture();

			_secretContext = new Mock<IStorageContext<Secret>>();
			_groupContext = new Mock<IStorageContext<Group>>();
			_transactionFactory = new Mock<ITransactionFactory>();

			_target = new GroupRepository(_groupContext.Object, _secretContext.Object, _transactionFactory.Object);
		}

		[Test]
		public void ExistsTest()
		{
			// arrange
			const string name = "test";
			const string path = "";

			var group = _fixture.Build<Group>().With(x => x.Name, name).With(x => x.Path, path).Create();

			_groupContext.Setup(x => x.Find(It.IsAny<Expression<Predicate<Group>>>())).Returns(group);

			// act
			var exists = _target.Exists(name, path);

			// assert
			exists.Should().BeTrue();
			_groupContext.Verify(x => x.Find(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);
		}

		[Test]
		public void NotExistsTest()
		{
			// arrange
			const string name = "test";
			const string path = "";

			_groupContext.Setup(x => x.Find(It.IsAny<Expression<Predicate<Group>>>())).Returns<Group>(null);

			// act
			var exists = _target.Exists(name, path);

			// assert
			exists.Should().BeFalse();
			_groupContext.Verify(x => x.Find(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);
		}

		[Test]
		public void CreateGroupTest()
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

			_transactionFactory.Setup(x => x.BeginTransaction()).Returns(mockedTransaction.Object);
			mockedTransaction.Setup(x => x.Commit()).Verifiable();

			_groupContext.Setup(x => x.Add(It.Is<Group>(y => y.Name == name && y.Path == path))).Returns(group);
			_secretContext.Setup(x => x.Add(It.IsAny<Secret>()));

			// act
			_target.CreateGroup(name, path, secrets, files);

			// assert
			_transactionFactory.Verify(x => x.BeginTransaction(), Times.Once);
			mockedTransaction.Verify(x => x.Commit(), Times.Once);

			_groupContext.Verify(x => x.Add(It.Is<Group>(y => y.Name == name && y.Path == path)), Times.Once);
			_secretContext.Verify(x => x.Add(It.IsAny<Secret>()),
				Times.Exactly(secrets.Keys.Count + files.Keys.Count));
		}

		[Test]
		public void DeleteGroupTest()
		{
			// arrange
			var group = _fixture.Create<Group>();
			var mockedTransaction = new Mock<IDatabaseTransactionScope>();

			_transactionFactory.Setup(x => x.BeginTransaction()).Returns(mockedTransaction.Object);
			mockedTransaction.Setup(x => x.Commit()).Verifiable();

			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(group);
			_secretContext.Setup(x => x.Delete(It.IsAny<Expression<Func<Secret, bool>>>())).Verifiable();
			_groupContext.Setup(x => x.Delete(It.IsAny<Expression<Func<Group, bool>>>())).Verifiable();

			// act
			_target.DeleteGroup(group.Name, group.Path);

			// arrange
			_transactionFactory.Verify(x => x.BeginTransaction(), Times.Once);
			mockedTransaction.Verify(x => x.Commit(), Times.Once);

			_groupContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);
			_secretContext.Verify(x => x.Delete(It.IsAny<Expression<Func<Secret, bool>>>()), Times.Once);
			_groupContext.Verify(x => x.Delete(It.IsAny<Expression<Func<Group, bool>>>()), Times.Once);
		}

		private IFixture _fixture;

		private Mock<IStorageContext<Group>> _groupContext;
		private Mock<IStorageContext<Secret>> _secretContext;
		private Mock<ITransactionFactory> _transactionFactory;

		private IGroupRepository _target;
	}
}