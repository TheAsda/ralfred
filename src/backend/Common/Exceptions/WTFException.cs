using System;


namespace Ralfred.Common.Exceptions
{
	public class WtfException : Exception
	{
		public WtfException() : base("This cannot happen") { }
	}
}