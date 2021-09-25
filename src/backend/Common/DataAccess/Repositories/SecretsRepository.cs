using System.Collections.Generic;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Helpers;


namespace Ralfred.Common.DataAccess.Repositories
{
	public sealed class SecretsRepository : ISecretsRepository
	{
		public SecretsRepository(IStorageContext<Secret> secretContext, IStorageContext<Group> groupContext, ITransactionFactory transactionFactory)
		{
			_secretContext = secretContext;
			_groupContext = groupContext;

			_transactionFactory = transactionFactory;
		}

		public IEnumerable<Secret> GetGroupSecrets(string groupName, string path)
		{
			var group = _groupContext.Get(x => x.Name == groupName && x.Path == path);

			return _secretContext.List(x => x.GroupId == group.Id);
		}

		public void UpdateGroupSecrets(string name, string path, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			var transaction = _transactionFactory.BeginTransaction();
			var group = _groupContext.Get(x => x.Name == name && x.Path == path);

			foreach (var (key, value) in secrets)
			{
				var secret = _secretContext.Get(x => x.GroupId == group.Id && x.Name == key);
				secret.Value = value;

				_secretContext.Update(secret);
			}

			foreach (var (key, value) in files)
			{
				var secret = _secretContext.Get(x => x.GroupId == group.Id && x.Name == key);
				secret.Value = value;

				_secretContext.Update(secret);
			}

			transaction.Commit();
		}

		public void SetGroupSecrets(string name, string path, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			var transaction = _transactionFactory.BeginTransaction();

			var group = _groupContext.Get(x => x.Name == name && x.Path == path);
			_secretContext.Delete(x => x.GroupId == group.Id);

			foreach (var (key, value) in secrets)
			{
				_secretContext.Add(new Secret
				{
					Name = key,
					Value = value,
					GroupId = group.Id,
					IsFile = false
				});
			}

			foreach (var (key, value) in files)
			{
				_secretContext.Add(new Secret
				{
					Name = key,
					Value = value,
					GroupId = group.Id,
					IsFile = true
				});
			}

			transaction.Commit();
		}

		public void DeleteGroupSecrets(string name, string path, IEnumerable<string> secrets)
		{
			var transaction = _transactionFactory.BeginTransaction();
			var group = _groupContext.Get(x => x.Name.Equals(name) && x.Path == path);

			foreach (var secret in secrets)
			{
				_secretContext.Delete(x => x.GroupId == group.Id && x.Name.Equals(secret));
			}

			transaction.Commit();
		}

		private readonly IStorageContext<Group> _groupContext;
		private readonly IStorageContext<Secret> _secretContext;

		private readonly ITransactionFactory _transactionFactory;
	}
}