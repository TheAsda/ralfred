using System.Collections.Generic;

using Microsoft.AspNetCore.Http;


namespace Ralfred.SecretsProvider.Services
{
	public interface IFileConverter
	{
		Dictionary<string, string> Convert(IFormCollection? form);
	}
}