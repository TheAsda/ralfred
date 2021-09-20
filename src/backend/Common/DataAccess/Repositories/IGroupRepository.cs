using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface IGroupRepository
	{
		bool Exists(string name, string path);

		void CreateGroup(string name, string path, Dictionary<string, string> secrets, Dictionary<string, string> files);

		void DeleteGroup(string name, string path);
	}
}