using System;
using System.Collections.Generic;

using DapperExtensions;
using DapperExtensions.Predicate;

using EnsureArg;

using Npgsql;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.DataAccess.Repositories.InMemory;
using Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration;
using Ralfred.Common.Types;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
{
	public class PostgresSecretRepository : BasePostgresRepository, ISecretsRepository
	{
		public PostgresSecretRepository(StorageConnection storageConnection, IConnectionFactory connectionFactory)
			: base(typeof(SecretMapper))
		{
			_storageConnection = storageConnection;
			_connectionFactory = connectionFactory;
		}

		public IEnumerable<Secret> GetGroupSecrets(Guid groupId)
		{
			Ensure.Arg(groupId).IsNotDefaultValue();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			return connection.GetList<Secret>(Predicates.Field<Secret>(x => x.GroupId, Operator.Eq, groupId));
		}

		public void UpdateGroupSecrets(Guid groupId, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			Ensure.Arg(groupId).IsNotDefaultValue();
			Ensure.Arg(secrets).IsNotNull();
			Ensure.Arg(files).IsNotNull();

			void UpdateSecret(NpgsqlConnection connection, string key, string value)
			{
				var secret = connection.Get<Secret>(Predicates.Group(GroupOperator.And, new IPredicate[]
				{
					Predicates.Field<Secret>(x => x.Name, Operator.Eq, key),
					Predicates.Field<Secret>(x => x.GroupId, Operator.Eq, groupId),
				}));

				secret.Value = value;

				connection.Update(secret);
			}

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			foreach (var (key, value) in secrets)
			{
				UpdateSecret(connection, key, value);
			}

			foreach (var (key, value) in files)
			{
				UpdateSecret(connection, key, value);
			}
		}

		public void SetGroupSecrets(Guid groupId, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			Ensure.Arg(groupId).IsNotDefaultValue();
			Ensure.Arg(secrets).IsNotNull();
			Ensure.Arg(files).IsNotNull();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			connection.Delete<Secret>(Predicates.Field<Secret>(x => x.GroupId, Operator.Eq, groupId));

			foreach (var (key, value) in secrets)
			{
				connection.Insert(new Secret
				{
					Name = key,
					Value = value,
					GroupId = groupId,
					Id = Guid.NewGuid(),
					IsFile = false
				});
			}

			foreach (var (key, value) in files)
			{
				connection.Insert(new Secret
				{
					Name = key,
					Value = value,
					GroupId = groupId,
					Id = Guid.NewGuid(),
					IsFile = true
				});
			}
		}

		public void DeleteGroupSecrets(Guid groupId, IEnumerable<string> secrets)
		{
			Ensure.Arg(groupId).IsNotDefaultValue();

			using var connection = _connectionFactory.Create(_storageConnection.ConnectionString);
			connection.Open();

			foreach (var secret in secrets)
			{
				var predicates = new IPredicate[]
				{
					Predicates.Field<Secret>(x => x.GroupId, Operator.Eq, groupId),
					Predicates.Field<Secret>(x => x.Name, Operator.Eq, secret),
				};

				connection.Delete(Predicates.Group(GroupOperator.And, predicates));
			}
		}

		private readonly StorageConnection _storageConnection;
		private readonly IConnectionFactory _connectionFactory;
	}
}