using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using MongoDB.Driver;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Context
{
	public class MongoStorageContext<T> : IStorageContext<T> where T : Entity
	{
		public MongoStorageContext(StorageConnection connectionSettings)
		{
			_connectionSettings = connectionSettings;
		}

		#region Implementation of IStorageContext

		public T Get(Expression<Predicate<T>> filter)
		{
			throw new NotImplementedException();
		}

		public T? Find(Expression<Predicate<T>> filter)
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

		public T Add(T entity)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> Delete(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public T Update(T entity)
		{
			throw new NotImplementedException();
		}

		#endregion

		private T Process(Func<IMongoClient, T> func)
		{
			_client ??= new MongoClient(_connectionSettings.ConnectionString);

			return func(_client);
		}

		private IMongoClient? _client;

		private readonly StorageConnection _connectionSettings;
	}
}