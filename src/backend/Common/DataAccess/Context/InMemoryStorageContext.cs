using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Exceptions;


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
				throw new NotFoundException("No item match criteria");
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
			if (entity.Id == Guid.Empty)
			{
				entity.Id = Guid.NewGuid();
			}
			else
			{
				if (_storage.Any(x => x.Id == entity.Id))
				{
					throw new ArgumentException("Id already exists");
				}
			}

			_storage.Add(entity);

			return entity;
		}

		public IEnumerable<T> Delete(Expression<Func<T, bool>> filter)
		{
			var items = _storage.Where(filter.Compile()).ToList();
			items.ForEach(x => _storage.Remove(x));

			return items;
		}

		public T? Update(T entity)
		{
			var index = _storage.FindIndex(x => x.Id == entity.Id);

			if (index == -1)
			{
				return null;
			}

			_storage[index] = entity;

			return entity;
		}

		#endregion

		private readonly List<T> _storage;
	}
}