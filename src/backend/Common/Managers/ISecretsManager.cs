using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.Managers
{
	public interface ISecretsManager
	{
		IEnumerable<Secret> GetSecrets(string path, string[] secrets);
	}
}