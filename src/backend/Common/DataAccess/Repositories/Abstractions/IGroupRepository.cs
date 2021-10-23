using System;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.Abstractions
{
	public interface IGroupRepository
	{
		bool Exists(string name, string path);

		Group Get(string name, string path);

		Guid CreateGroup(string name, string path);

		void DeleteGroup(string name, string path);
	}
}