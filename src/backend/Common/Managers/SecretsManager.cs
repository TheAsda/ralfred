using System;
using System.Collections.Generic;
using System.Linq;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.Exceptions;
using Ralfred.Common.Helpers;
using Ralfred.Common.Types;


namespace Ralfred.Common.Managers
{
	public class SecretsManager : ISecretsManager
	{
		public SecretsManager(IPathResolver pathResolver, ISecretsRepository secretsRepository, IGroupRepository groupRepository)
		{
			_pathResolver = pathResolver;
			_secretsRepository = secretsRepository;
			_groupRepository = groupRepository;
		}

		public IEnumerable<Secret> GetSecrets(string path, string[] secrets)
		{
			var pathType = _pathResolver.Resolve(path);

			switch (pathType)
			{
				case PathType.Secret:
				{
					var (name, groupPath) = _pathResolver.DeconstructPath(path);
					var (groupName, folderPath) = _pathResolver.DeconstructPath(groupPath);
					var groupSecrets = _secretsRepository.GetGroupSecrets(groupName, folderPath ?? string.Empty);

					var secret = groupSecrets.FirstOrDefault(x => x.Name == name);

					if (secret is null)
					{
						throw new NotFoundException("Group does not contain such secret");
					}

					return new[] { secret };
				}
				case PathType.Group:
				{
					var (groupName, folderPath) = _pathResolver.DeconstructPath(path);

					return _secretsRepository.GetGroupSecrets(groupName, folderPath ?? string.Empty)
						.Where(x => secrets.Length == 0 || secrets.Contains(x.Name));
				}
				case PathType.None:
					throw new NotFoundException("Path not found");
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void AddSecrets(string path, Dictionary<string, string> input, Dictionary<string, string> files, string[] secrets)
		{
			var pathType = _pathResolver.Resolve(path);

			switch (pathType)
			{
				case PathType.None:
				{
					var (groupName, folderPath) = _pathResolver.DeconstructPath(path);

					_groupRepository.CreateGroup(groupName, folderPath ?? string.Empty,
						FilterDictionaryKeys(input, secrets),
						FilterDictionaryKeys(files, secrets));

					break;
				}
				case PathType.Group:
				{
					var (groupName, folderPath) = _pathResolver.DeconstructPath(path);

					if (secrets.Length > 0)
					{
						_secretsRepository.UpdateGroupSecrets(groupName, folderPath ?? string.Empty,
							FilterDictionaryKeys(input, secrets),
							FilterDictionaryKeys(files, secrets));
					}
					else
					{
						_secretsRepository.SetGroupSecrets(groupName, folderPath ?? string.Empty, input, files);
					}

					break;
				}
				case PathType.Secret:
				{
					if (!input.ContainsKey("value") && !files.ContainsKey("value"))
					{
						throw new ArgumentException("Value is not provided");
					}

					var (name, groupPath) = _pathResolver.DeconstructPath(path);
					var (groupName, folderPath) = _pathResolver.DeconstructPath(groupPath);

					if (input.ContainsKey("value"))
					{
						_secretsRepository.UpdateGroupSecrets(groupName, folderPath,
							new Dictionary<string, string> { { name, input["value"] } },
							new Dictionary<string, string>());
					}
					else
					{
						_secretsRepository.UpdateGroupSecrets(groupName, folderPath,
							new Dictionary<string, string>(),
							new Dictionary<string, string> { { name, files["value"] } });
					}

					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void DeleteSecrets(string path, string[] secrets)
		{
			var pathType = _pathResolver.Resolve(path);

			switch (pathType)
			{
				case PathType.Secret:
				{
					var (secretName, groupPath) = _pathResolver.DeconstructPath(path);
					var (groupName, folderPath) = _pathResolver.DeconstructPath(groupPath);
					_secretsRepository.DeleteGroupSecrets(groupName, folderPath, new[] { secretName });

					break;
				}
				case PathType.Group:
				{
					var (secretName, groupPath) = _pathResolver.DeconstructPath(path);

					if (secrets.Length > 0)
					{
						_secretsRepository.DeleteGroupSecrets(secretName, groupPath, secrets);
					}
					else
					{
						_groupRepository.DeleteGroup(secretName, groupPath);
					}

					break;
				}
				case PathType.None:
					throw new NotFoundException("Path not found");
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private Dictionary<string, string> FilterDictionaryKeys(Dictionary<string, string> dictionary, string[] keys) =>
			keys.Any()
				? dictionary
					.Where(x => keys.Contains(x.Key))
					.ToDictionary(x => x.Key, x => x.Value)
				: dictionary;

		private readonly IPathResolver _pathResolver;
		private readonly ISecretsRepository _secretsRepository;
		private readonly IGroupRepository _groupRepository;
	}
}