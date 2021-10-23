using System;

using DapperExtensions;
using DapperExtensions.Predicate;

using EnsureArg;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.DataAccess.Repositories.InMemory.EntityConfiguration;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public class PostgresGroupRepository : BasePostgresRepository, IGroupRepository
	{
		public PostgresGroupRepository(StorageConnection storageConnection, IConnectionFactory connectionFactory) : base(typeof(RoleMapper))
		{
			_storageConnection = storageConnection;
			_connectionFactory = connectionFactory;
		}

		public bool Exists(string name, string path)
		{
			Ensure.Arg(name).IsNotNullOrWhiteSpace();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			var predicates = new IPredicate[]
			{
				Predicates.Field<Group>(x => x.Name, Operator.Eq, name),
				Predicates.Field<Group>(x => x.Path, Operator.Eq, path)
			};

			return connection.Get<Group>(Predicates.Group(GroupOperator.And, predicates)) is null;
		}

		public Group Get(string name, string path)
		{
			Ensure.Arg(name).IsNotNullOrWhiteSpace();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
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
			Ensure.Arg(name).IsNotNullOrWhiteSpace();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
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
			Ensure.Arg(name).IsNotNullOrWhiteSpace();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			var predicates = new IPredicate[]
			{
				Predicates.Field<Group>(x => x.Name, Operator.Eq, name),
				Predicates.Field<Group>(x => x.Path, Operator.Eq, path)
			};

			connection.Delete(Predicates.Group(GroupOperator.And, predicates));
		}

		private readonly StorageConnection _storageConnection;
		private readonly IConnectionFactory _connectionFactory;
	}
}