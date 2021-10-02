namespace Ralfred.Common.Security
{
	public interface ITokenProvider
	{
		string Generate(int days);
	}
}