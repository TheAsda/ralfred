using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class GroupRepository
	{
		public GroupRepository(IStorageContext<Group> storageContext)
		{
			_storageContext = storageContext;
		}

		private readonly IStorageContext<Group> _storageContext;
	}
}