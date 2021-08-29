using Microsoft.AspNetCore.Mvc;

using Ralfred.SecretsProvider.Models;


namespace Ralfred.SecretsProvider.Controllers
{
	[ApiController]
	[Route("{*route}")]
	public class SecretsController : ControllerBase
	{
		[HttpPut]
		public void AddSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',');
			var secretPath = payload.Route.Split("/");
		}

		[HttpDelete]
		public void RemoveSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',');
			var secretPath = payload.Route.Split("/");
		}
	}
}