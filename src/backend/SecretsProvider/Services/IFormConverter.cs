using System.Collections.Generic;

using Microsoft.AspNetCore.Http;


namespace Ralfred.SecretsProvider.Services
{
	public interface IFormConverter
	{
		Dictionary<string, string> Convert(IFormCollection? form);
	}
}