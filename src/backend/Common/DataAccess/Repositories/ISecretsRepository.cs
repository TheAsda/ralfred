using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface ISecretsRepository
	{
		void AddSecret(string name, string value);
	}
}