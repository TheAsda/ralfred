using System;


namespace Ralfred.Common.Types
{
	[Serializable]
	public record StorageConnection
	{
		public string ConnectionString { get; init; } = default!;
	}
}