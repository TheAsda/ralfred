using LinqToDB;
using LinqToDB.Mapping;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration
{
	internal sealed class SecretMapper : IMapper
	{
		public void Apply(MappingSchema schema)
		{
			schema.GetFluentMappingBuilder()
				.Entity<Secret>()
				.HasSchemaName("public")
				.HasTableName("secret")
				.Property(x => x.Id).HasColumnName(nameof(Secret.Id).ToLower()).IsPrimaryKey().HasDataType(DataType.Guid)
				.Property(x => x.Name).HasColumnName(nameof(Secret.Name).ToLower())
				.Property(x => x.Value).HasColumnName(nameof(Secret.Value).ToLower())
				.Property(x => x.GroupId).HasColumnName(nameof(Secret.GroupId).ToLower()).HasDataType(DataType.Guid)
				.Property(x => x.IsFile).HasColumnName(nameof(Secret.IsFile).ToLower());
		}
	}
}