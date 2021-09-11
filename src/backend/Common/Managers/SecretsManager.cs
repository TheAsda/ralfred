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
		public SecretsManager(IPathResolver pathResolver, ISecretsRepository secretsRepository)
		{
			_pathResolver = pathResolver;
			_secretsRepository = secretsRepository;
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
					var groupSecrets = _secretsRepository.GetGroupSecrets(groupName, folderPath);

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

					return _secretsRepository.GetGroupSecrets(groupName, folderPath);
				}
				case PathType.None:
					// TODO: change to custom exception
					throw new Exception("Path not found");
				default:
					throw new WtfException();
			}
		}

		private readonly IPathResolver _pathResolver;
		private readonly ISecretsRepository _secretsRepository;
	}
}