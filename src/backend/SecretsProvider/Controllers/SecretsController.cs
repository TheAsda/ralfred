using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Ralfred.Controllers
{
	[ApiController]
	public class SecretsController : ControllerBase
	{
		[HttpPut("{*route}")]
		// TODO: add body payload
		public void AddSecrets([FromForm] IFormCollection formPayload, [FromQuery] string? secrets, [FromRoute] string route)
		{
			var secretNames = secrets?.Split(',');
			var secretPath = route.Split("/");
		}
		
		[HttpDelete("{*route}")]
		// TODO: add body payload
		public void RemoveSecrets([FromForm] IFormCollection formPayload, [FromQuery] string? secrets, [FromRoute] string route)
		{
			var secretNames = secrets?.Split(',');
			var secretPath = route.Split("/");
		}
	}
}