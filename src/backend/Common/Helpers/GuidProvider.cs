using System;


namespace Ralfred.Common.Helpers
{
	public class GuidProvider : IGuidProvider
	{
		public Guid Generate()
		{
			return Guid.NewGuid();
		}
	}
}