using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Helpers;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class GroupRepository : IGroupRepository
	{
		public GroupRepository(IStorageContext<Group> storageContext, IPathResolver pathResolver)
		{
			_storageContext = storageContext;
			_pathResolver = pathResolver;
		}

		#region Implementation of IGroupRepository

		public bool Exists(string path)
		{
			var (groupName, groupPath) = _pathResolver.DeconstructPath(path);
			var group = _storageContext.Find(g => g.Path.Equals(groupPath) && g.Name.Equals(groupName));

			return group != null;
		}

		public Group? FindByPath(string path, string name)
		{
			var group = _storageContext.Find(g => g.Path.Equals(path) && g.Name.Equals(name));

			return group;
		}

		#endregion

		private readonly IPathResolver _pathResolver;
		private readonly IStorageContext<Group> _storageContext;
	}
}