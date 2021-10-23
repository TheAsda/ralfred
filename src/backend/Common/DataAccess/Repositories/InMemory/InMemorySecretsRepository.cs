using System;
using System.Collections.Generic;
using System.Linq;

using EnsureArg;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class InMemorySecretsRepository : ISecretsRepository
	{
		public InMemorySecretsRepository()
		{
			_storage = new List<Secret>();
		}

		public IEnumerable<Secret> GetGroupSecrets(Guid groupId)
		{
			Ensure.Arg(groupId).IsNotDefaultValue();

			return _storage.Where(x => x.GroupId.Equals(groupId));
		}

		public void UpdateGroupSecrets(Guid groupId, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			Ensure.Arg(groupId).IsNotDefaultValue();
			Ensure.Arg(secrets).IsNotNull();
			Ensure.Arg(files).IsNotNull();

			foreach (var (key, value) in secrets)
			{
				var secret = _storage.Single(x => x.GroupId == groupId && x.Name == key);
				secret.Value = value;

				UpdateSecret(secret);
			}

			foreach (var (key, value) in files)
			{
				var secret = _storage.Single(x => x.GroupId == groupId && x.Name == key);
				secret.Value = value;

				UpdateSecret(secret);
			}
		}

		public void SetGroupSecrets(Guid groupId, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			Ensure.Arg(groupId).IsNotDefaultValue();
			Ensure.Arg(secrets).IsNotNull();
			Ensure.Arg(files).IsNotNull();

			DeleteSecretByGroupId(groupId);

			foreach (var (key, value) in secrets)
			{
				_storage.Add(new Secret
				{
					Name = key,
					Value = value,
					GroupId = groupId,
					Id = Guid.NewGuid(),
					IsFile = false
				});
			}

			foreach (var (key, value) in files)
			{
				_storage.Add(new Secret
				{
					Name = key,
					Value = value,
					GroupId = groupId,
					Id = Guid.NewGuid(),
					IsFile = true
				});
			}
		}

		public void DeleteGroupSecrets(Guid groupId, IEnumerable<string> secrets)
		{
			Ensure.Arg(groupId).IsNotDefaultValue();

			foreach (var secret in secrets)
			{
				DeleteSecretByGroupIdAndName(groupId, secret);
			}
		}

		private void UpdateSecret(Secret secret)
		{
			var index = _storage.FindIndex(x => x.Id == secret.Id);

			if (index == -1)
			{
				return;
			}

			_storage[index] = secret;
		}

		private void DeleteSecretByGroupId(Guid groupId)
		{
			var items = _storage.Where(x => x.GroupId.Equals(groupId)).ToList();
			items.ForEach(x => _storage.Remove(x));
		}

		private void DeleteSecretByGroupIdAndName(Guid groupId, string secret)
		{
			var items = _storage.Where(x => x.GroupId.Equals(groupId) && x.Name.Equals(secret, StringComparison.OrdinalIgnoreCase))
				.ToList();

			items.ForEach(x => _storage.Remove(x));
		}

		private readonly List<Secret> _storage;
	}
}