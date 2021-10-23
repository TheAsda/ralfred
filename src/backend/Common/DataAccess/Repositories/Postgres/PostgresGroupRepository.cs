using System;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresGroupRepository : IGroupRepository
	{
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
	}
}