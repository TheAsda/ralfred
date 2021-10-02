namespace Ralfred.Common.Security
{
	public interface ITokenValidator
	{
		bool Validate(string token);
	}
}