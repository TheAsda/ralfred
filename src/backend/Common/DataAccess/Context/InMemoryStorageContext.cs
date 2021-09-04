using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Context
{
	public class InMemoryStorageContext<T> : IStorageContext<T> where T : Entity
	{
		public InMemoryStorageContext(List<T> storage)
		{
			_storage = storage;
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

		public void Add(T entity)
		{
			_storage.Add(entity);
		}

		public void Delete(T entity)
		{
			_storage = _storage.Where(x => x.Id == entity.Id).ToList();
		}

		public void Update(T entity)
		{
			var index = _storage.FindIndex(x => x.Id == entity.Id);
			_storage[index] = entity;
		}

		#endregion

		private List<T> _storage;
	}
}