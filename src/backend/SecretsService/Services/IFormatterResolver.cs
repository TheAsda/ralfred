using Ralfred.Common.Types;
using Ralfred.SecretsProvider.Services.Formatters;


namespace Ralfred.SecretsProvider.Services
{
	public interface IFormatterResolver
	{
		ISecretFormatter Resolve(FormatType? type);
	}
}