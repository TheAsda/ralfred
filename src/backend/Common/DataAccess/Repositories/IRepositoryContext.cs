using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface IRepositoryContext
	{
		ISecretsRepository GetSecretsRepository();

		IAccountRepository GetAccountRepository();

		IGroupRepository GetGroupRepository();

		IRolesRepository GetRolesRepository();
	}
}