using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public sealed class SecretsRepository : ISecretsRepository
	{
		public SecretsRepository(IStorageContext<Secret> storageContext)
		{
			_storageContext = storageContext;
		}

		private readonly IStorageContext<Secret> _storageContext;
	}
}