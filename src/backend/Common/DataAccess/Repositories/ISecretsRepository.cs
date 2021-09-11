using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface ISecretsRepository
	{
		IEnumerable<Secret> GetGroupSecrets(string groupName, string path);
	}
}