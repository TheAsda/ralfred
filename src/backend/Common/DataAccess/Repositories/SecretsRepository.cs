using System.Collections.Generic;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public sealed class SecretsRepository : ISecretsRepository
	{
		public SecretsRepository(IStorageContext<Secret> secretsContext, IStorageContext<Group> groupContext)
		{
			_secretsContext = secretsContext;
			_groupContext = groupContext;
		}

		public IEnumerable<Secret> GetGroupSecrets(string groupName, string path)
		{
			var group = _groupContext.Get(x => x.Name == groupName && x.Path == path);

			return _secretsContext.List(x => x.GroupId == group.Id);
		}

		private readonly IStorageContext<Group> _groupContext;
		private readonly IStorageContext<Secret> _secretsContext;
	}
}