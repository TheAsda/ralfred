using System;
using System.Linq;
using System.Text.RegularExpressions;

using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.Types;


namespace Ralfred.Common.Resolvers
{
	public class PathResolver
	{
		public PathResolver(IGroupRepository groupRepository)
		{
			_groupRepository = groupRepository;
		}

		public PathType GetPathType(string path)
		{
			ValidatePath(path);

			// Find group with full path (path/name)
			var fullPathGroup = _groupRepository.FindByFullPath(path);

			// If group is found then provided path leads to a group
			if (fullPathGroup is not null)
			{
				return PathType.Group;
			}

			// Trying to find group with path except the last element in it 
			var pathGroup = _groupRepository.FindByFullPath(string.Join("/", path.Split("/")[..^1]));

			// If group is found it means that the excepted element is the name of a secret
			if (pathGroup is null)
			{
				return PathType.Secret;
			}

			// Nothing found
			return PathType.None;
		}

		private void ValidatePath(string path)
		{
			var isValid = new Regex(@"^(/\w+)+/?$").IsMatch(path);

			if (!isValid)
			{
				// TODO: create custom exception
				throw new Exception("Path is not valid");
			}
		}

		private readonly IGroupRepository _groupRepository;
	}
}