using System;


namespace Ralfred.Common.Exceptions
{
	public class RestrictedAccessException : Exception
	{
		public RestrictedAccessException(string message) : base(message) { }
	}
}