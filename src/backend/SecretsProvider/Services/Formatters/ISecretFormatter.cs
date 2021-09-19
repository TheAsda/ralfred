using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.SecretsProvider.Services.Formatters
{
	public interface ISecretFormatter
	{
		string? Format(IEnumerable<Secret> data);
	}
}