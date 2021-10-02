using System;


namespace Ralfred.Common.Helpers
{
	public interface IDateTimeProvider
	{
		DateTime GetUtc();
	}
}