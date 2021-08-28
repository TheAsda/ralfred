using Microsoft.AspNetCore.Mvc;

using Ralfred.Modules;


namespace Ralfred.Controllers
{
	[ApiController]
	[Route("system")]
	public class SystemController : ControllerBase
	{
		[HttpGet("status")]
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
		public Config GetConfig()
		{
			// TODO: add real config
			return new Config();
		}
	}
}