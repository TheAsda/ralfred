using System.Collections.Generic;
using System.IO;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.Managers
{
	public interface ISecretsManager
	{
		IEnumerable<Secret> GetSecrets(string path, string[] secrets);

		void AddSecret(string path, Dictionary<string, string> input, Dictionary<string, string> files, string[] secrets);
	}
}