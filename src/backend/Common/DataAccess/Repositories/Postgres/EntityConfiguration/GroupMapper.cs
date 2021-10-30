using LinqToDB;
using LinqToDB.Mapping;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration
{
	internal sealed class GroupMapper : IMapper
	{
		public void Apply(MappingSchema schema)
		{
			schema.GetFluentMappingBuilder()
				.Entity<Group>()
				.HasSchemaName("public")
				.HasTableName("group")
				.Property(x => x.Id).HasColumnName(nameof(Group.Id).ToLower()).IsPrimaryKey().HasDataType(DataType.Guid)
				.Property(x => x.Name).HasColumnName(nameof(Group.Name).ToLower())
				.Property(x => x.Path).HasColumnName(nameof(Group.Path).ToLower());
		}
	}
}