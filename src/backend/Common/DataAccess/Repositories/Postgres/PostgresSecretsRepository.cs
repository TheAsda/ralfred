using System;
using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresSecretsRepository : ISecretsRepository
	{
		public IEnumerable<Secret> GetGroupSecrets(Guid groupId)
		{
			throw new System.NotImplementedException();
		}

		public void UpdateGroupSecrets(Guid groupId, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			throw new System.NotImplementedException();
		}

		public void SetGroupSecrets(Guid groupId, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteGroupSecrets(Guid groupId, IEnumerable<string> secrets)
		{
			throw new System.NotImplementedException();
		}
	}
}