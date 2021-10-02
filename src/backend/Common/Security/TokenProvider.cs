using System;
using System.Linq;

using Ralfred.Common.Helpers;


namespace Ralfred.Common.Security
{
	public class TokenProvider : ITokenProvider
	{
		public TokenProvider(IGuidProvider guidProvider, IDateTimeProvider dateTimeProvider)
		{
			_guidProvider = guidProvider;
			_dateTimeProvider = dateTimeProvider;
		}

		public string Generate(int days)
		{
			var time = BitConverter.GetBytes(_dateTimeProvider.GetUtc().AddDays(days).ToBinary());
			var payload = _guidProvider.Generate().ToByteArray();

			var token = Convert.ToBase64String(time.Concat(payload).ToArray());

			return token;
		}

		private readonly IGuidProvider _guidProvider;
		private readonly IDateTimeProvider _dateTimeProvider;
	}
}