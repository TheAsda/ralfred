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

		public ISecretsRepository GetSecretsRepository()
		{
			return _serviceProvider.GetService<InMemorySecretsRepository>()!;
		}

		public IAccountRepository GetAccountRepository()
		{
			return _serviceProvider.GetService<InMemoryAccountRepository>()!;
		}

		public IGroupRepository GetGroupRepository()
		{
			return _serviceProvider.GetService<InMemoryGroupRepository>()!;
		}

		public IRolesRepository GetRolesRepository()
		{
			return _serviceProvider.GetService<InMemoryRolesRepository>()!;
		}

		private readonly IServiceProvider _serviceProvider;
	}
}