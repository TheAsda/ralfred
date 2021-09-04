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

		public T Get(Expression<Func<T, bool>> filter)
		{
			return _storage.First(filter.Compile());
		}

		public T? Find(Expression<Func<T, bool>> filter)
		{
			return _storage.FirstOrDefault(filter.Compile());
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
			entity.Id = GetNextId();
			_storage.Add(entity);

			return entity;
		}

		public T Delete(int id)
		{
			var entity = _storage.First(x => x.Id == id);
			_storage.Remove(entity);

			return entity;
		}

		public T Update(T entity)
		{
			var index = _storage.FindIndex(x => x.Id == entity.Id);
			_storage[index] = entity;

			return entity;
		}

		#endregion

		private int GetNextId()
		{
			var ids = _storage.Select(x => x.Id).ToList();
			ids.Sort((a, b) => a - b);

			try
			{
				return ids.Last() + 1;
			}
			catch
			{
				return 0;
			}
		}

		private List<T> _storage;
	}
}