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
			return new SystemInfo
			{
				Version = "0.0.0",
				StorageType = "Inmemory"
			};
		}
	}
}