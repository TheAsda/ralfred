using System;

using DapperExtensions;
using DapperExtensions.Predicate;

using EnsureThat;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.DataAccess.Repositories.InMemory;
using Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public class PostgresGroupRepository : BasePostgresRepository, IGroupRepository
	{
		public PostgresGroupRepository(IConnectionFactory connectionFactory) : base(typeof(RoleMapper))
		{
			_connectionFactory = connectionFactory;
		}

		public bool Exists(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			using var connection = _connectionFactory.Create();
			connection.Open();

			var predicates = new IPredicate[]
			{
				Predicates.Field<Group>(x => x.Name, Operator.Eq, name),
				Predicates.Field<Group>(x => x.Path, Operator.Eq, path)
			};

			return connection.Get<Group>(Predicates.Group(GroupOperator.And, predicates)) is not null;
		}

		public Group Get(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			using var connection = _connectionFactory.Create();
			connection.Open();

			var predicates = new IPredicate[]
			{
				Predicates.Field<Group>(x => x.Name, Operator.Eq, name),
				Predicates.Field<Group>(x => x.Path, Operator.Eq, path)
			};

			return connection.Get<Group>(Predicates.Group(GroupOperator.And, predicates));
		}

		public Guid CreateGroup(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			using var connection = _connectionFactory.Create();
			connection.Open();

			var group = new Group
			{
				Id = Guid.NewGuid(),
				Name = name,
				Path = path
			};

			connection.Insert(group);

			return group.Id;
		}

		public void DeleteGroup(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			using var connection = _connectionFactory.Create();
			connection.Open();

			var predicates = new IPredicate[]
			{
				Predicates.Field<Group>(x => x.Name, Operator.Eq, name),
				Predicates.Field<Group>(x => x.Path, Operator.Eq, path)
			};

			connection.Delete<Group>(Predicates.Group(GroupOperator.And, predicates));
		}

		private readonly IConnectionFactory _connectionFactory;
	}
}