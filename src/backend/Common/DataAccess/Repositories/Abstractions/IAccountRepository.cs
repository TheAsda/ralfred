using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.Abstractions
{
	public interface IAccountRepository
	{
		bool Exists(string accountName);

		void Add(Account account);

		Account? GetByName(string accountName);

		Account? Update(Account account);
	}
}