using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Context
{
	public interface IStorageContext<T> where T : Entity
	{
		T Get(Expression<Func<T, bool>> filter);

		T? Find(Expression<Func<T, bool>> filter);

		IEnumerable<T> List();

		IEnumerable<T> List(Expression<Func<T, bool>> filter);

		void Add(T entity);

		void Delete(T entity);

		void Update(T entity);
	}
}