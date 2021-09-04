using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Resolvers;
using Ralfred.Common.Types;
using Ralfred.SecretsProvider.Models;


namespace Ralfred.SecretsProvider.Controllers
{
	[ApiController]
	[Route("{*route}")]
	public class SecretsController : ControllerBase
	{
		public SecretsController(Providers.SecretsProvider secretsProvider, PathResolver pathResolver)
		{
			_secretsProvider = secretsProvider;
			_pathResolver = pathResolver;
		}

		[HttpPut]
		public void AddSecrets([FromRoute] RequestPayload payload)
		{
			if (payload.Body is null && payload.FormData is null)
			{
				throw new Exception("Body or form data is not provided");
			}

			var secretPath = payload.Route;
			var pathType = _pathResolver.GetPathType(secretPath);

			// var secretNames = payload.Secrets?.Split(',');
			if (pathType == PathType.None)
			{
				if (payload.FormData is not null)
				{
					var dict = new Dictionary<string, string>();

					foreach (var key in payload.FormData.Keys)
					{
						dict.Add(key, payload.FormData[key]);
					}

					// TODO: add encoding files to base64
					_secretsProvider.CreateGroup(secretPath, dict);
				}
				else
				{
					_secretsProvider.CreateGroup(secretPath, payload.Body);
				}

				return;
			}

			if (pathType == PathType.Group)
			{
				if (payload.FormData is not null)
				{
					var dict = new Dictionary<string, string>();

					// TODO: handle picked secrets
					// TODO: add encoding files to base64
					_secretsProvider.UpdateSecrets(secretPath, dict);
				}
				else
				{
					_secretsProvider.UpdateSecrets(secretPath, payload.Body);
				}

				return;
			}

			if (pathType == PathType.Secret)
			{
				string value;

				if (payload.FormData is not null)
				{
					if (!payload.FormData.TryGetValue("value", out _))
					{
						// TODO: change to custom exception
						throw new Exception("Value is not provided");
					}

					value = payload.FormData["value"];
				}
				else
				{
					if (!payload.Body.ContainsKey("value"))
					{
						// TODO: change to custom exception
						throw new Exception("Value is not provided");
					}

					value = payload.Body["value"];
				}

				var array = secretPath.Split('/');
				var path = string.Join("", array[..^1]);
				var secretName = array.Last();

				_secretsProvider.UpdateSecrets(path, new Dictionary<string, string>
				{
					{ secretName, value }
				});

				return;
			}

			throw new Exception("WTF");
		}

		[HttpDelete]
		public void RemoveSecrets([FromRoute] RequestPayload payload)
		{
			var fullPath = payload.Route;

			var pathType = _pathResolver.GetPathType(fullPath);

			if (pathType == PathType.None)
			{
				// TODO: change to custom exception 
				throw new Exception("Path not found");
			}

			if (pathType == PathType.Group)
			{
				if (payload.Secrets is not null)
				{
					var pickedSecretNames = payload.Secrets.Split(',');

					foreach (var name in pickedSecretNames)
					{
						_secretsProvider.RemoveSecret(fullPath, name);
					}
				}
				else
				{
					_secretsProvider.RemoveGroup(fullPath);
				}

				return;
			}

			if (pathType == PathType.Secret)
			{
				var (name, path) = PathResolver.SplitPath(fullPath);
				_secretsProvider.RemoveSecret(path, name);

				return;
			}

			throw new Exception("WTF");
		}

		[HttpGet]
		public Dictionary<string, string> GetSecrets([FromRoute] RequestPayload payload)
		{
			var fullPath = payload.Route;
			var pathType = _pathResolver.GetPathType(fullPath);

			if (pathType == PathType.None)
			{
				// TODO: change to custom exception
				throw new Exception("Path not found");
			}

			if (pathType == PathType.Group)
			{
				var secrets = _secretsProvider.GetGroupSecrets(fullPath);

				if (payload.Secrets is not null)
				{
					var pickedSecretNames = payload.Secrets.Split(',');
					secrets = secrets.Where(secret => pickedSecretNames.Contains(secret.Name));
				}

				return secrets
					.ToDictionary(secret => secret.Name, secret => secret.Value);
			}

			if (pathType == PathType.Secret)
			{
				var (name, path) = PathResolver.SplitPath(fullPath);

				var secrets = _secretsProvider.GetGroupSecrets(path);
				var secret = secrets.FirstOrDefault(x => x.Name == name);

				if (secret is null)
				{
					throw new Exception("Secret not found");
				}

				return new Dictionary<string, string> { { secret.Name, secret.Value } };
			}

			throw new Exception("WFT");
		}

		private readonly PathResolver _pathResolver;
		private readonly Providers.SecretsProvider _secretsProvider;
	}
}