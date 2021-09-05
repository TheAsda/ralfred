using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Context
{
	public class InMemoryStorageContext<T> : IStorageContext<T> where T : Entity
	{
		public InMemoryStorageContext()
		{
			_storage = new List<T>();
		}

		#region Implementation of IStorageContext

		public T Get(Expression<Predicate<T>> filter)
		{
			var found = _storage.Find(filter.Compile());

			if (found is null)
			{
				// TODO: change to custom exception
				throw new Exception("No item match criteria");
			}

			return found;
		}

		public T? Find(Expression<Predicate<T>> filter)
		{
			return _storage.Find(filter.Compile());
		}

		public IEnumerable<T> List()
		{
			return _storage;
		}

		public IEnumerable<T> List(Expression<Func<T, bool>> filter)
		{
			return _storage.Where(filter.Compile());
		}

		public T Add(T entity)
		{
			if (entity.Id == default)
			{
				entity.Id = Guid.NewGuid();
			}

			_storage.Add(entity);

			return entity;
		}

		public T Delete(Expression<Predicate<T>> filter)
		{
			var index = _storage.FindIndex(filter.Compile());

			if (index == -1)
			{
				// TODO: change to custom exception
				throw new Exception("No item match criteria");
			}

			var item = _storage[index];
			_storage.RemoveAt(index);

			return item;
		}

		public T Update(T entity)
		{
			if (entity.Id == default)
			{
				throw new ArgumentException("Id is not provided");
			}

			var index = _storage.FindIndex(x => x.Id == entity.Id);

			if (index == -1)
			{
				// TODO: change to custom exception
				throw new Exception("No item witch such id");
			}

			_storage[index] = entity;

			return entity;
		}

		#endregion

		private readonly List<T> _storage;
	}
}