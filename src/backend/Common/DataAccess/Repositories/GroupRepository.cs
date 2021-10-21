using System.Collections.Generic;

using EnsureArg;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class GroupRepository : IGroupRepository
	{
		public GroupRepository(IStorageContext<Group> groupContext, IStorageContext<Secret> secretContext)
		{
			_groupContext = groupContext;
			_secretContext = secretContext;
		}

		#region Implementation of IGroupRepository

		public bool Exists(string name, string path)
		{
			Ensure.Arg(name).IsNotNullOrWhiteSpace();

			var group = _groupContext.Find(g => g.Path.Equals(path) && g.Name.Equals(name));

			return group is not null;
		}

		public Group? FindByPath(string path, string name)
		{
			Ensure.Arg(name).IsNotNullOrWhiteSpace();

			var group = _groupContext.Find(g => g.Path.Equals(path) && g.Name.Equals(name));

			return group;
		}

		public void CreateGroup(string name, string path, Dictionary<string, string> secrets, Dictionary<string, string> files)
		{
			Ensure.Arg(name).IsNotNullOrWhiteSpace();
			Ensure.Arg(secrets).IsNotNull();
			Ensure.Arg(files).IsNotNull();

			var group = _groupContext.Add(new Group
			{
				Name = name,
				Path = path
			});

			// TODO: add transaction
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
		}

		public void DeleteGroup(string name, string path)
		{
			Ensure.Arg(name).IsNotNullOrWhiteSpace();

			var group = _groupContext.Get(x => x.Name.Equals(name) && x.Path == path);

			_secretContext.Delete(x => x.GroupId == group.Id);
			_groupContext.Delete(x => x.Id == group.Id);
		}

		#endregion

		private readonly IStorageContext<Group> _groupContext;
		private readonly IStorageContext<Secret> _secretContext;
	}
}