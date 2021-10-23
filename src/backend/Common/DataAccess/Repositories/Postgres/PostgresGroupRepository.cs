using System;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.DataAccess.Repositories.InMemory.EntityConfiguration;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresGroupRepository : BasePostgresRepository, IGroupRepository
	{
		public PostgresGroupRepository(StorageConnection storageConnection) : base(typeof(RoleMapper))
		{
			_storageConnection = storageConnection;
		}

		public bool Exists(string name, string path)
		{
			throw new System.NotImplementedException();
		}

		public Group Get(string name, string path)
		{
			throw new NotImplementedException();
		}

		public Guid CreateGroup(string name, string path)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteGroup(string name, string path)
		{
			throw new System.NotImplementedException();
		}

		private readonly StorageConnection _storageConnection;
	}
}