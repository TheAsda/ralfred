﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Npgsql;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Context
{
	public class PostgreStorageContext<T> : IStorageContext<T> where T : Entity
	{
		public PostgreStorageContext(StorageConnection connectionSettings)
		{
			_connectionSettings = connectionSettings;
		}

		#region Implementation of IStorageContext

		public T Get(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public T? Find(Expression<Func<T, bool>> filter)
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
			using var connection = new NpgsqlConnection(_connectionSettings.ConnectionString);

			connection.Open();

			return func(connection);
		}

		private readonly StorageConnection _connectionSettings;
	}
}