using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.SecretsService.Services.Formatters
{
	public interface ISecretFormatter
	{
		string? Format(IEnumerable<Secret> data);
	}
}