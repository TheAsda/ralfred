using System.Data;

using DapperExtensions.Mapper;

using Ralfred.Common.DataAccess.Entities;


internal sealed class AccountMapper : ClassMapper<Account>
{
	public AccountMapper()
	{
		Schema("public");
		Table("account");

		Map(x => x.Id).Column(nameof(Account.Id).ToLower()).Type(DbType.Guid).Key(KeyType.Assigned);

		Map(x => x.Name).Column(nameof(Account.Name).ToLower()).Type(DbType.String);
		Map(x => x.TokenHash).Column(nameof(Account.TokenHash).ToLower()).Type(DbType.String);
		Map(x => x.CertificateThumbprint).Column(nameof(Account.CertificateThumbprint).ToLower()).Type(DbType.String);

		Map(x => x.RoleIds).Ignore();

		AutoMap();
	}
}