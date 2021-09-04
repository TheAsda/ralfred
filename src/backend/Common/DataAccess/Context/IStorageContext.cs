using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

		T Add(T entity);

		T Delete(int id);

		T Update(T entity);
	}
}