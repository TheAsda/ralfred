using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.SecretsProvider.Services.Formatters
{
	public interface IContentFormatter
	{
		string? Format(IEnumerable<Secret> data);
	}
}