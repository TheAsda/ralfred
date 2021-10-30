using System.Data;

using DapperExtensions.Mapper;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration
{
	internal sealed class GroupMapper : ClassMapper<Group>
	{
		public GroupMapper()
		{
			Schema("public");
			Table("group");

			Map(x => x.Id).Column(nameof(Group.Id).ToLower()).Type(DbType.Guid).Key(KeyType.Assigned);

			Map(x => x.Name).Column(nameof(Group.Name).ToLower()).Type(DbType.String);
			Map(x => x.Path).Column(nameof(Group.Path).ToLower()).Type(DbType.String);
		}
	}
}