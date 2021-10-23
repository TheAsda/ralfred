using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class PostgreRepositoryContext : IRepositoryContext
	{
		public ISecretsRepository GetSecretsRepository()
		{
			throw new System.NotImplementedException();
		}

		public IAccountRepository GetAccountRepository()
		{
			throw new System.NotImplementedException();
		}

		public IGroupRepository GetGroupRepository()
		{
			throw new System.NotImplementedException();
		}

		public IRolesRepository GetRolesRepository()
		{
			throw new System.NotImplementedException();
		}
	}
}