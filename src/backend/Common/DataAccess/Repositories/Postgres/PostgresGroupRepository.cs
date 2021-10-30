using System;
using System.Linq;

using EnsureThat;

using LinqToDB;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.Exceptions;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public class PostgresGroupRepository : IGroupRepository
	{
		private readonly IConnectionFactory _connectionFactory;

		public PostgresGroupRepository(IConnectionFactory connectionFactory) =>
			_connectionFactory = connectionFactory;

		public bool Exists(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			using var connection = _connectionFactory.Create();

			return connection.GetTable<Group>().SingleOrDefault(x => x.Name.Equals(name) && x.Path.Equals(path)) is not null;
		}

		public Group Get(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			using var connection = _connectionFactory.Create();

			try
			{
				return connection.GetTable<Group>().Single(x => x.Name.Equals(name) && x.Path.Equals(path));
			}
			catch (InvalidOperationException)
			{
				throw new NotFoundException("Group not found");
			}
		}

		public Guid CreateGroup(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			using var connection = _connectionFactory.Create();

			var group = new Group
			{
				Id = Guid.NewGuid(),
				Name = name,
				Path = path
			};

			connection.GetTable<Group>().Insert(() => new Group
			{
				Id = Guid.NewGuid(),
				Name = name,
				Path = path
			});

			return group.Id;
		}

		public void DeleteGroup(string name, string path)
		{
			EnsureArg.IsNotEmptyOrWhiteSpace(name);

			using var connection = _connectionFactory.Create();

			connection.GetTable<Group>().Delete(x => x.Name.Equals(name) && x.Path.Equals(path));
		}
	}
}