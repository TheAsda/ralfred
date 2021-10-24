using System.Data;

using DapperExtensions.Mapper;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration
{
	internal sealed class SecretMapper : ClassMapper<Secret>
	{
		public SecretMapper()
		{
			Table("secret");

			Map(x => x.Id).Column(nameof(Secret.Id).ToLower()).Type(DbType.Guid).Key(KeyType.Guid);

			Map(x => x.Name).Column(nameof(Secret.Name).ToLower()).Type(DbType.String);
			Map(x => x.Value).Column(nameof(Secret.Value).ToLower()).Type(DbType.String);
			Map(x => x.GroupId).Column(nameof(Secret.GroupId).ToLower()).Type(DbType.Guid);
			Map(x => x.IsFile).Column(nameof(Secret.IsFile).ToLower()).Type(DbType.Boolean);
		}
	}
}