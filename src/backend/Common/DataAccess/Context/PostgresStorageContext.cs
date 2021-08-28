using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Npgsql;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Context
{
	public class PostgreStorageContext<T> : IStorageContext<T> where T : Entity
	{
		public PostgreStorageContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		#region Implementation of IStorageContext

		public T Get()
		{
			throw new NotImplementedException();
		}

		public T Get(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> List()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> List(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public void Add(T entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(T entity)
		{
			throw new NotImplementedException();
		}

		public void Update(T entity)
		{
			throw new NotImplementedException();
		}

		#endregion

		private T Process(Func<NpgsqlConnection, T> func)
		{
			using var connection = new NpgsqlConnection(_connectionString);

			connection.Open();

			return func(connection);
		}

		private readonly string _connectionString;
	}
}