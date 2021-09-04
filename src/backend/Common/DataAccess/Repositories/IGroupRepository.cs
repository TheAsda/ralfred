using System.Collections.Generic;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories
{
	public interface IGroupRepository
	{
		Group? FindByFullPath(string fullPath);
	}
}