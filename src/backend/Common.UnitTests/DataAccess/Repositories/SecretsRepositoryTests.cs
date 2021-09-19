using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AutoFixture;

using Moq;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;


namespace SecretsProvider.UnitTests.DataAccess.Repositories
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
			_target = new SecretsRepository(_secretContext.Object, _groupContext.Object);
		}

		[Test]
		public void GetGroupSecretsTest()
		{
			// arrange
			var mockGroup = new Group
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Path = ""
			};

			var mockSecret = new Secret
			{
				Id = Guid.NewGuid(),
				Name = "testSecret",
				Value = "test",
				GroupId = mockGroup.Id,
				IsFile = false
			};

			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(mockGroup);
			_secretContext.Setup(x => x.List(It.IsAny<Expression<Func<Secret, bool>>>())).Returns(new List<Secret> { mockSecret });

			// act
			var result = _target.GetGroupSecrets(mockGroup.Path, mockGroup.Name).ToList();

			// assert
			Assert.AreEqual(result.Count, 1);
			Assert.AreEqual(result[0], mockSecret);
		}

		[Test]
		public void UpdateGroupSecretsTest()
		{
			// arrange
			const string name = "test";
			const string path = "";

			var mockGroup = new Group
			{
				Id = Guid.NewGuid(),
				Name = name,
				Path = path
			};

			var secrets = new Dictionary<string, string>
			{
				{ "test", "test" },
				{ "test2", "test2" }
			};

			var files = new Dictionary<string, string>
			{
				{ "file", "file" }
			};

			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(mockGroup);

			var mockSecret = new Secret
			{
				Id = Guid.NewGuid(),
				Value = "oldValue",
			};

			_secretContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Secret>>>())).Returns(mockSecret);
			_secretContext.Setup(x => x.Update(It.IsAny<Secret>()));

			// act
			_target.UpdateGroupSecrets(name, path, secrets, files);

			// assert
			_groupContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);

			_secretContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Secret>>>()),
				Times.Exactly(secrets.Keys.Count + files.Keys.Count));

			_secretContext.Verify(x => x.Update(It.IsAny<Secret>()), Times.Exactly(secrets.Keys.Count + files.Keys.Count));
		}

		[Test]
		public void SetGroupSecretsTest()
		{
			// arrange
			const string name = "test";
			const string path = "";

			var mockGroup = new Group
			{
				Id = Guid.NewGuid(),
				Name = name,
				Path = path
			};

			var secrets = new Dictionary<string, string>
			{
				{ "test", "test" },
				{ "test2", "test2" }
			};

			var files = new Dictionary<string, string>
			{
				{ "file", "file" }
			};

			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(mockGroup);

			var mockSecret = new Secret
			{
				Id = Guid.NewGuid(),
				Value = "oldValue",
			};

			_secretContext.Setup(x => x.Add(It.IsAny<Secret>()));

			// act
			_target.SetGroupSecrets(name, path, secrets, files);

			// assert
			_groupContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);

			_secretContext.Verify(x => x.Add(It.IsAny<Secret>()), Times.Exactly(secrets.Keys.Count + files.Keys.Count));
		}

		[Test]
		public void DeleteGroupSecretsTest()
		{
			// arrange
			var group = _fixture.Create<Group>();
			var secrets = _fixture.Build<Secret>().With(x => x.GroupId, () => group.Id).CreateMany().ToList();
			_groupContext.Setup(x => x.Get(It.IsAny<Expression<Predicate<Group>>>())).Returns(group);
			_secretContext.Setup(x => x.Delete(It.IsAny<Expression<Func<Secret, bool>>>())).Verifiable();

			// act
			var enumerable = secrets.ToList();
			_target.DeleteGroupSecrets(group.Name, group.Path, enumerable.Select(x => x.Name));

			// assert
			_groupContext.Verify(x => x.Get(It.IsAny<Expression<Predicate<Group>>>()), Times.Once);
			_secretContext.Verify(x => x.Delete(It.IsAny<Expression<Func<Secret, bool>>>()), Times.Exactly(secrets.Count));
		}

		private IFixture _fixture;

		private ISecretsRepository _target;
		private Mock<IStorageContext<Secret>> _secretContext;
		private Mock<IStorageContext<Group>> _groupContext;
	}
}