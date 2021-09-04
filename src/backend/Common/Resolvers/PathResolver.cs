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

		public PathType GetPathType(string fullPath)
		{
			ValidatePath(fullPath);

			// Find group with full path (path/name)
			var fullPathGroup = _groupRepository.FindByFullPath(fullPath);

			// If group is found then provided path leads to a group
			if (fullPathGroup is not null)
			{
				return PathType.Group;
			}

			var (_, path) = SplitPath(fullPath);
			// Trying to find group with path except the last element in it 
			var pathGroup = _groupRepository.FindByFullPath(path);

			// If group is found it means that the excepted element is the name of a secret
			if (pathGroup is null && fullPath.Split("/").Length > 1)
			{
				return PathType.Secret;
			}

			// Nothing found
			return PathType.None;
		}

		public static (string name, string path) SplitPath(string fullPath)
		{
			var array = fullPath.Split('/');
			var path = string.Join("", array[..^1]);
			var name = array.Last();

			return (name, path);
		}

		private static void ValidatePath(string path)
		{
			var isValid = new Regex(@"^[a-zA-Z0-9\-_]+(\/[a-zA-Z0-9\-_]+)*$").IsMatch(path);

			if (!isValid)
			{
				// TODO: create custom exception
				throw new Exception("Path is not valid");
			}
		}

		private readonly IGroupRepository _groupRepository;
	}
}