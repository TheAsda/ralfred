using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ralfred.Common.DataAccess.Context;
using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public class GroupRepository : IGroupRepository
	{
		public GroupRepository(IStorageContext<Group> groupStorageContext, IStorageContext<Secret> secretStorageContext)
		{
			_groupStorageContext = groupStorageContext;
			_secretStorageContext = secretStorageContext;
		}

		public Group? FindByFullPath(string fullPath)
		{
			var array = fullPath.Split('/');
			var path = string.Join("/", array[..^1]);
			var name = array.Last();
			var group = _groupStorageContext.Find(g => g.Path == path && g.Name == name);

			if (group is null)
			{
				return null;
			}

			group.Secrets = _secretStorageContext.List(x => x.GroupId == group.Id).ToArray();

			return group;
		}

		public void SetSecrets(int id, Dictionary<string, string> secrets)
		{
			// Check if group exists
			_groupStorageContext.Get(x => x.Id == id);

			foreach (var secret in secrets)
			{
				_secretStorageContext.Add(new Secret
				{
					Name = secret.Key,
					Value = secret.Value,
					GroupId = id
				});
			}
		}

		public Group CreateGroup(string name, string path)
		{
			return _groupStorageContext.Add(new Group
			{
				Name = name,
				Path = path
			});
		}

		public Group CreateGroup(string fullPath)
		{
			var array = fullPath.Split('/');
			var path = string.Join("/", array[..^1]);
			var name = array.Last();

			return _groupStorageContext.Add(new Group
			{
				Name = name,
				Path = path,
			});
		}

		private readonly IStorageContext<Secret> _secretStorageContext;
		private readonly IStorageContext<Group> _groupStorageContext;
	}
}