using System;
using System.IO;
using System.Linq;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class GroupRepository : IGroupRepository
	{
		public GroupRepository(IStorageContext<Group> storageContext)
		{
			_storageContext = storageContext;
		}

		public Group? FindByFullPath(string fullPath)
		{
			var array = fullPath.Split('/');
			var path = string.Join("/", array[..^1]);
			var name = array.Last();
			
			return _storageContext.Find(g => g.Path == path && g.Name == name);
		}

		private readonly IStorageContext<Group> _storageContext;
	}
}