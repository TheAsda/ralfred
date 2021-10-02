using System;


namespace Ralfred.Common.Types
{
	[Serializable]
	public record Configuration
	{
		/* Common parameters */
		public bool? EnableWebUi { get; set; }

		public FormatType? DefaultFormat { get; set; }


		/* Storage configuration */
		public StorageEngineType? Engine { get; set; }

		public string? ConnectionString { get; set; }

		/* Authentication configuration */
		public string? RootToken { get; set; }
	}
}