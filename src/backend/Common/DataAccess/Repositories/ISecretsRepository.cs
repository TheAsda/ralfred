using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface ISecretsRepository
	{
		IEnumerable<Secret> GetGroupSecrets(string groupName, string path);

		void UpdateGroupSecrets(string name, string path, Dictionary<string, string> secrets, Dictionary<string, string> files);

		void SetGroupSecrets(string name, string path, Dictionary<string, string> secrets, Dictionary<string, string> files);
	}
}