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

		public void AddSecret(string name, string value)
		{
			_storageContext.Add(new Secret
			{
				Name = name, 
				Value = value,
				IsFile = false
			});
		}

		private readonly IStorageContext<Secret> _storageContext;
	}
}