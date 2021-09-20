using Microsoft.AspNetCore.Http;


namespace Ralfred.SecretsProvider.Models
{
	public record Error
	{
		public int StatusCode { get; set; }

		public string Message { get; set; }
	}
}