using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Ralfred.Common.Managers;
using Ralfred.SecretsProvider.Models;
using Ralfred.SecretsProvider.Services;


namespace Ralfred.SecretsProvider.Controllers
{
	[ApiController]
	[Route("{*route}")]
	public class SecretsController : ControllerBase
	{
		public SecretsController(ISecretsManager secretsManager, IFormConverter formConverter)
		{
			_secretsManager = secretsManager;
			_formConverter = formConverter;
		}

		[HttpPut]
		public void AddSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',') ?? new string[] { };
			var form = _formConverter.Convert(payload.FormData);

			_secretsManager.AddSecrets(payload.Route,
				payload.Body ?? new Dictionary<string, string>(),
				form,
				secretNames);
		}

		[HttpDelete]
		public void RemoveSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',');
			var secretPath = payload.Route.Split("/");
		}

		private readonly ISecretsManager _secretsManager;
		private readonly IFormConverter _formConverter;
	}
}