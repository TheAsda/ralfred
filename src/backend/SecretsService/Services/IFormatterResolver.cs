using Ralfred.Common.Types;
using Ralfred.SecretsService.Services.Formatters;


namespace Ralfred.SecretsService.Services
{
	public interface IFormatterResolver
	{
		ISecretFormatter Resolve(FormatType? type);
	}
}