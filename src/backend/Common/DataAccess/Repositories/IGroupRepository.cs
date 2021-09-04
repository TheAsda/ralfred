using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface IGroupRepository
	{
		Group? FindByFullPath(string fullPath);

		void SetSecrets(int id, Dictionary<string, string> secrets);

		Group CreateGroup(string name, string path);
		Group CreateGroup(string fullPath);
	}
}