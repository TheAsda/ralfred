namespace Ralfred.SecretsProvider.Models
{
	public record SystemInfo
	{
		public string Version { get; set; }

		public string StorageType { get; set; }
	}
}