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

		public void CreateGroup(string path, Dictionary<string, string> secrets)
		{
			var group = _groupRepository.CreateGroup(path);
			_groupRepository.SetSecrets(group.Id, secrets);
		}

		public void UpdateSecrets(string path, Dictionary<string, string> secrets)
		{
			// Throw exception if group is not found 
			var group = _groupRepository.FindByFullPath(path);

			if (group is null)
			{
				throw new Exception("Group not found");
			}

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

		public void RemoveSecret(string path, string name)
		{
			var group = _groupRepository.FindByFullPath(path);

			if (group is null)
			{
				throw new Exception("Group not found");
			}

			_groupRepository.RemoveSecret(group.Id, name);
		}

		public void RemoveGroup(string path)
		{
			_groupRepository.RemoveGroup(path);
		}

		private readonly ISecretsRepository _secretsRepository;
		private readonly IGroupRepository _groupRepository;
	}
}