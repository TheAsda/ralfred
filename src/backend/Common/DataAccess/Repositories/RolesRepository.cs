using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public sealed class RolesRepository : IRolesRepository
	{
		public RolesRepository(IStorageContext<Role> storageContext)
		{
			_storageContext = storageContext;
		}

		private readonly IStorageContext<Role> _storageContext;
	}
}