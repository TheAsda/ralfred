using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface IGroupRepository
	{
		Group? FindByFullPath(string fullPath);

		void SetSecrets(int   id, Dictionary<string, string> secrets);
		void RemoveSecret(int id, string                     name);

		Group CreateGroup(string name, string path);
		Group CreateGroup(string fullPath);
		void  RemoveGroup(string path);
	}
}