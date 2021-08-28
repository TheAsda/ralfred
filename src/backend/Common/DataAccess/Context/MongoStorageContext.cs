using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using MongoDB.Driver;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Context
{
	public class MongoStorageContext<T> : IStorageContext<T> where T : Entity
	{
		public MongoStorageContext(string connectionString)
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

		private T Process(Func<IMongoClient, T> func)
		{
			_client ??= new MongoClient(_connectionString);

			return func(_client);
		}

		private IMongoClient? _client;

		private readonly string _connectionString;
	}
}