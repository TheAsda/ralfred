using Microsoft.AspNetCore.Mvc;

using Ralfred.SecretsProvider.Models;


namespace Ralfred.SecretsProvider.Controllers
{
	[ApiController]
	[Route("system")]
	public class SystemController : ControllerBase
	{
		[HttpGet("status")]
		[ProducesResponseType(200)]
		public SystemInfo GetSystemInfo()
		{
			// TODO: add getting version and storage type
			return new SystemInfo
			{
				Version = "0.0.0",
				StorageType = "Inmemory"
			};
		}

		[HttpGet("config")]
		[ProducesResponseType(200)]
		public Config GetConfig()
		{
			// TODO: add real config
			return new Config();
		}

		[HttpGet("start")]
		[ProducesResponseType(200)]
		public void Start()
		{
			// TODO: add start logic
			return;
		}

		[HttpGet("stop")]
		[ProducesResponseType(200)]
		public void Stop()
		{
			// TODO: add stop logic
			return;
		}
	}
}