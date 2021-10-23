using System.Data;

using DapperExtensions.Mapper;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.InMemory.EntityConfiguration
{
	internal sealed class RoleMapper : ClassMapper<Role>
	{
		public RoleMapper()
		{
			Table("role");

			Map(x => x.Id).Column(nameof(Role.Id).ToLower()).Type(DbType.Guid).Key(KeyType.Guid);
		}
	}
}