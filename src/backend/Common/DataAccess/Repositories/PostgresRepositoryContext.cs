using System;

using Microsoft.Extensions.DependencyInjection;

using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.DataAccess.Repositories.Postgres;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class PostgresRepositoryContext : IRepositoryContext
	{
		private readonly IServiceProvider _serviceProvider;

		public PostgresRepositoryContext(IServiceProvider serviceProvider) =>
			_serviceProvider = serviceProvider;

		public ISecretsRepository GetSecretRepository()
		{
			return _serviceProvider.GetService<PostgresSecretRepository>()!;
		}

		public IAccountRepository GetAccountRepository()
		{
			return _serviceProvider.GetService<PostgresAccountRepository>()!;
		}

		public IGroupRepository GetGroupRepository()
		{
			return _serviceProvider.GetService<PostgresGroupRepository>()!;
		}

		public IRolesRepository GetRoleRepository()
		{
			return _serviceProvider.GetService<PostgresRoleRepository>()!;
		}
	}
}