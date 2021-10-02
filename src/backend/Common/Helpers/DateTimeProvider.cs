using System;


namespace Ralfred.Common.Helpers
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime GetUtc()
		{
			return DateTime.UtcNow;
		}
	}
}