using System;
using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;


namespace Ralfred.SecretsProvider.Providers
{
	public class SecretsProvider
	{
		public SecretsProvider(ISecretsRepository secretsRepository, IGroupRepository groupRepository)
		{
			_secretsRepository = secretsRepository;
			_groupRepository = groupRepository;
		}

		public void AddSecrets(string path, Dictionary<string, string> secrets)
		{
			// Throw exception if group is not found 
			var group = _groupRepository.FindByFullPath(path) ?? _groupRepository.CreateGroup(path);

			_groupRepository.SetSecrets(group.Id, secrets);
		}

		public IEnumerable<Secret> GetGroupSecrets(string path)
		{
			var group = _groupRepository.FindByFullPath(path);

			if (group is null)
			{
				throw new Exception("Group not found");
			}

			return group.Secrets;
		}

		private readonly ISecretsRepository _secretsRepository;
		private readonly IGroupRepository _groupRepository;
	}
}