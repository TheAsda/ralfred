using Ralfred.Common.Types;
using Ralfred.SecretsProvider.Services.Formatters;


namespace Ralfred.SecretsProvider.Services
{
	public interface IFormatterResolver
	{
		IContentFormatter Resolve(FormatType? type);
	}
}