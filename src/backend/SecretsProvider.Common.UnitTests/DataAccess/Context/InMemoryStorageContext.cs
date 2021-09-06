using System;
using System.Linq;

using NUnit.Framework;
using NUnit.Framework.Internal;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace SecretsProvider.UnitTests.DataAccess.Context
{
	[TestFixture]
	public class InMemoryStorageContext
	{
		[SetUp]
		public void Setup()
		{
			_target = new InMemoryStorageContext<TestEntity>();
		}

		[Test]
		public void EmptyOnInitTest()
		{
			// arrange

			// act
			var entities = _target.List();

			//assert
			Assert.IsEmpty(entities);
		}

		[Test]
		public void AddTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			// act
			var entity = _target.Add(newEntity);

			// assert
			Assert.AreEqual(entity.Name, newEntity.Name);
			Assert.AreEqual(entity.Age, newEntity.Age);
			Assert.AreNotEqual(entity.Id, Guid.Empty);
		}

		[Test]
		public void AddWithIdTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Age = 0
			};

			// act
			var entity = _target.Add(newEntity);

			// assert
			Assert.AreEqual(entity.Name, newEntity.Name);
			Assert.AreEqual(entity.Age, newEntity.Age);
			Assert.AreEqual(entity.Id, newEntity.Id);
		}

		[Test]
		public void GetTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			var entity = _target.Add(newEntity);

			// act
			var found = _target.Get(x => x.Id == entity.Id);

			// assert
			Assert.AreEqual(entity.Name, found.Name);
			Assert.AreEqual(entity.Age, found.Age);
			Assert.AreEqual(entity.Id, found.Id);
		}

		[Test]
		public void GetNotFoundTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			_target.Add(newEntity);

			// act

			// assert			
			Assert.Throws<Exception>(() => _target.Get(x => x.Name == "random"));
		}

		[Test]
		public void FindTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			var entity = _target.Add(newEntity);

			// act
			var found = _target.Find(x => x.Id == entity.Id);

			// assert
			Assert.IsNotNull(found);
			Assert.AreEqual(entity.Name, found.Name);
			Assert.AreEqual(entity.Age, found.Age);
			Assert.AreEqual(entity.Id, found.Id);
		}

		[Test]
		public void FindNullTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			_target.Add(newEntity);

			// act
			var found = _target.Find(x => x.Name == "random");

			// assert
			Assert.IsNull(found);
		}

		[Test]
		public void ListTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			_target.Add(newEntity);

			// act
			var list = _target.List();

			// assert
			Assert.AreEqual(list.Count(), 1);
		}

		[Test]
		public void FilteredListTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			var entity = _target.Add(newEntity);

			var newEntity2 = new TestEntity
			{
				Name = "test2",
				Age = 0
			};

			_target.Add(newEntity2);

			// act
			var found = _target.List(x => x.Name == entity.Name).ToList();

			// assert
			Assert.AreEqual(found.Count, 1);
			Assert.AreEqual(entity.Name, found[0].Name);
			Assert.AreEqual(entity.Age, found[0].Age);
			Assert.AreEqual(entity.Id, found[0].Id);
		}

		[Test]
		public void EmptyListTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			var entity = _target.Add(newEntity);

			var newEntity2 = new TestEntity
			{
				Name = "test2",
				Age = 0
			};

			_target.Add(newEntity2);

			// act
			var found = _target.List(x => x.Name == "random").ToList();

			// assert
			Assert.AreEqual(found.Count, 0);
		}

		[Test]
		public void DeleteTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			var entity = _target.Add(newEntity);

			// act
			var deleted = _target.Delete(x => x.Id == entity.Id);

			// assert
			Assert.AreEqual(entity.Name, deleted.Name);
			Assert.AreEqual(entity.Age, deleted.Age);
			Assert.AreEqual(entity.Id, deleted.Id);
		}

		[Test]
		public void DeleteNotFoundTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			_target.Add(newEntity);

			// act

			// assert
			Assert.Throws<Exception>(() => _target.Delete(x => x.Name == "random"));
		}

		[Test]
		public void UpdateTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			var entity = _target.Add(newEntity);

			var updatedEntity = new TestEntity
			{
				Id = entity.Id,
				Name = "updated",
				Age = 10
			};

			// act
			var updated = _target.Update(updatedEntity);

			// assert
			Assert.AreEqual(updatedEntity.Name, updated.Name);
			Assert.AreEqual(updatedEntity.Age, updated.Age);
			Assert.AreEqual(updatedEntity.Id, updated.Id);
		}

		[Test]
		public void UpdateWithoutIdTest()
		{
			// arrange
			var newEntity = new TestEntity
			{
				Name = "test",
				Age = 0
			};

			_target.Add(newEntity);

			var updatedEntity = new TestEntity
			{
				Name = "updated",
				Age = 10
			};

			// act

			// assert
			Assert.Throws<ArgumentException>(() => _target.Update(updatedEntity));
		}

		private IStorageContext<TestEntity> _target;
	}

	public record TestEntity : Entity
	{
		public string Name { get; set; }

		public uint Age { get; set; }
	}
}