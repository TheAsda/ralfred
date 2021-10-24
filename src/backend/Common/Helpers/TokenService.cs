using System;


namespace Ralfred.Common.Helpers
{
	public class TokenService : ITokenService
	{
		public string GenerateToken()
		{
			return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
		}
	}
}