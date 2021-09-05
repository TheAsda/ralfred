using System;

using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.Extensions;
using Ralfred.Common.Types;


namespace Ralfred.SecretsProvider.Helpers
{
	public class PathResolver : IPathResolver
	{
		public PathResolver(IGroupRepository groupRepository)
		{
			_groupRepository = groupRepository;
		}

		#region Implementation of IPathResolver

		public PathType Resolve(string path)
		{
			if (!path.ValidatePath())
			{
				throw new Exception("Path is not valid");
			}

			if (_groupRepository.Exists(path))
			{
				return PathType.Group;
			}

			var (_, aboveGroupPath) = path.DeconstructPath();

			if (string.IsNullOrWhiteSpace(aboveGroupPath))
			{
				return PathType.None;
			}

			if (_groupRepository.Exists(aboveGroupPath))
			{
				return PathType.Secret;
			}

			return PathType.None;
		}

		#endregion

		private readonly IGroupRepository _groupRepository;
	}
}