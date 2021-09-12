using System;
using System.Collections.Generic;
using System.Linq;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;
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
					var groupSecrets = _secretsRepository.GetGroupSecrets(groupName, folderPath ?? "");

					var secret = groupSecrets.FirstOrDefault(x => x.Name == name);

					if (secret is null)
					{
						// TODO: change to custom exception
						throw new Exception("Group does not contain such secret");
					}

					return new[] { secret };
				}
				case PathType.Group:
				{
					var (groupName, folderPath) = _pathResolver.DeconstructPath(path);

					return _secretsRepository.GetGroupSecrets(groupName, folderPath ?? "")
						.Where(x => secrets.Length == 0 || secrets.Contains(x.Name));
				}
				case PathType.None:
					// TODO: change to custom exception
					throw new Exception("Path not found");
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
					_groupRepository.CreateGroup(groupName, folderPath ?? "", input, files);

					break;
				}
				case PathType.Group:
				{
					var (groupName, folderPath) = _pathResolver.DeconstructPath(path);

					if (secrets.Length > 0)
					{
						_secretsRepository.UpdateGroupSecrets(groupName, folderPath ?? "",
							input.Where(x => secrets.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value),
							files.Where(x => secrets.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value));
					}
					else
					{
						_secretsRepository.SetGroupSecrets(groupName, folderPath ?? "", input, files);
					}

					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private readonly IPathResolver _pathResolver;
		private readonly ISecretsRepository _secretsRepository;
		private readonly IGroupRepository _groupRepository;
	}
}