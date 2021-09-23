namespace Ralfred.SecretsService.Models
{
	public record Error
	{
		public int StatusCode { init; get; }

		public string Message { init; get; }
	}
}