using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Types;

using StackExchange.Redis;


namespace Ralfred.Common.DataAccess.Context
{
	public class RedisStorageContext<T> : IStorageContext<T> where T : Entity
	{
		public RedisStorageContext(StorageConnection connectionSettings)
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

		private T Process(Func<IDatabase, T> func)
		{
			_connection ??= ConnectionMultiplexer.Connect(_connectionSettings.ConnectionString);

			return func(_connection.GetDatabase());
		}

		private IConnectionMultiplexer? _connection;

		private readonly StorageConnection _connectionSettings;
	}
}