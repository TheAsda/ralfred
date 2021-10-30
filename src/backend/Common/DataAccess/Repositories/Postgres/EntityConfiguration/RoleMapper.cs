using LinqToDB;
using LinqToDB.Mapping;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration
{
	internal sealed class RoleMapper : IMapper
	{
		public void Apply(MappingSchema schema)
		{
			schema.GetFluentMappingBuilder().Entity<Role>()
				.HasSchemaName("public")
				.HasTableName("role")
				.Property(x => x.Id).HasColumnName(nameof(Role.Id).ToLower()).IsPrimaryKey().HasDataType(DataType.Guid);
		}
	}
}