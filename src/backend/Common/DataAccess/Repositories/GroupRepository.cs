using System.Collections.Generic;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Helpers;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class GroupRepository : IGroupRepository
	{
		public GroupRepository(IStorageContext<Group> groupContext, IStorageContext<Secret> secretContext, ITransactionFactory transactionFactory)
		{
			_groupContext = groupContext;
			_secretContext = secretContext;

			_transactionFactory = transactionFactory;
		}

		#region Implementation of IGroupRepository

		public bool Exists(string name, string path)
		{
			var group = _groupContext.Find(g => g.Path.Equals(path) && g.Name.Equals(name));

			return group is not null;
		}

		public Group? FindByPath(string path, string name)
		{
			var group = _groupContext.Find(g => g.Path.Equals(path) && g.Name.Equals(name));

			return group;
		}

		public void CreateGroup(string name, string path, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			var transaction = _transactionFactory.BeginTransaction();
			var group = _groupContext.Add(new Group
			{
				Name = name,
				Path = path
			});

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

		public void DeleteGroup(string name, string path)
		{
			var transaction = _transactionFactory.BeginTransaction();
			var group = _groupContext.Get(x => x.Name.Equals(name) && x.Path == path);

			_secretContext.Delete(x => x.GroupId == group.Id);
			_groupContext.Delete(x => x.Id == group.Id);

			transaction.Commit();
		}

		#endregion

		private readonly IStorageContext<Group> _groupContext;
		private readonly IStorageContext<Secret> _secretContext;

		private readonly ITransactionFactory _transactionFactory;
	}
}