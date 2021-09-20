using System.Collections.Generic;

using Microsoft.AspNetCore.Http;


namespace Ralfred.SecretsService.Services
{
	public interface IFileConverter
	{
		Dictionary<string, string> Convert(IFormCollection? form);
	}
}