using System.Data;

using DapperExtensions.Mapper;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.InMemory.EntityConfiguration
{
	internal sealed class GroupMapper : ClassMapper<Group>
	{
		public GroupMapper()
		{
			Table("group");

			Map(x => x.Id).Column(nameof(Group.Id).ToLower()).Type(DbType.Guid).Key(KeyType.Guid);

			Map(x => x.Name).Column(nameof(Group.Name).ToLower()).Type(DbType.String);
			Map(x => x.Path).Column(nameof(Group.Path).ToLower()).Type(DbType.String);
		}
	}
}