using System;


namespace Ralfred.Common.Exceptions
{
	public class UnauthorizedException : Exception
	{
		public UnauthorizedException(string message) : base(message) { }
	}
}