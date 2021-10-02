using System;

using Ralfred.Common.Helpers;


namespace Ralfred.Common.Security
{
	public class TokenValidator : ITokenValidator
	{
		public TokenValidator(IDateTimeProvider dateTimeProvider)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public bool Validate(string token)
		{
			byte[] data;

			try
			{
				data = Convert.FromBase64String(token);
			}
			catch (Exception)
			{
				return false;
			}

			var when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

			return when > _dateTimeProvider.GetUtc();
		}

		private readonly IDateTimeProvider _dateTimeProvider;
	}
}