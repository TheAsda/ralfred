using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Context
{
	public interface IStorageContext<T> where T : Entity
	{
		T Get(Expression<Predicate<T>> filter);

		T? Find(Expression<Predicate<T>> filter);

		IEnumerable<T> List();

		IEnumerable<T> List(Expression<Func<T, bool>> filter);

		T Add(T entity);

		IEnumerable<T> Delete(Expression<Func<T, bool>> filter);

		T? Update(T entity);
	}
}