using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.Managers
{
	public interface ISecretsManager
	{
		IEnumerable<Secret> GetSecrets(string path, string[] secrets);

		void AddSecrets(string path, Dictionary<string, string> input, Dictionary<string, string> files, string[] secrets);

		void DeleteSecrets(string path, string[] secrets);
	}
}