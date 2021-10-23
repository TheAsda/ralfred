using Ralfred.Common.DataAccess.Repositories.Abstractions;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface IRepositoryContext
	{
		ISecretsRepository GetSecretRepository();

		IAccountRepository GetAccountRepository();

		IGroupRepository GetGroupRepository();

		IRolesRepository GetRoleRepository();
	}
}