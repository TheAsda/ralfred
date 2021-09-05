using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface IGroupRepository
	{
		bool Exists(string path);

		Group? FindByPath(string path, string name);
	}
}