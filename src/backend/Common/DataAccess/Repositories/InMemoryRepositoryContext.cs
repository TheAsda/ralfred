using System;

using Microsoft.Extensions.DependencyInjection;

using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.DataAccess.Repositories.InMemory;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class InMemoryRepositoryContext : IRepositoryContext
	{
		public InMemoryRepositoryContext(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public ISecretsRepository GetSecretRepository()
		{
			return _serviceProvider.GetService<InMemorySecretRepository>()!;
		}

		public IAccountRepository GetAccountRepository()
		{
			return _serviceProvider.GetService<InMemoryAccountRepository>()!;
		}

		public IGroupRepository GetGroupRepository()
		{
			return _serviceProvider.GetService<InMemoryGroupRepository>()!;
		}

		public IRolesRepository GetRoleRepository()
		{
			return _serviceProvider.GetService<InMemoryRoleRepository>()!;
		}

		private readonly IServiceProvider _serviceProvider;
	}
}