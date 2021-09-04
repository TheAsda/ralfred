using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Resolvers;
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
			if (payload.Body is null)
			{
				throw new Exception("Body or form data is not provided");
			}

			var secretPath = payload.Route;
			// var pathType = _pathResolver.GetPathType(secretPath);

			// var secretNames = payload.Secrets?.Split(',');

			_secretsProvider.AddSecrets(secretPath, payload.Body);
		}

		[HttpDelete]
		public void RemoveSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',');
			var secretPath = payload.Route;
		}

		[HttpGet]
		public IEnumerable<Secret> GetSecrets([FromRoute] RequestPayload payload)
		{
			var secretPath = payload.Route;
			var secrets = _secretsProvider.GetGroupSecrets(secretPath);
			return secrets;
		}

		private readonly PathResolver _pathResolver;
		private readonly Providers.SecretsProvider _secretsProvider;
	}
}