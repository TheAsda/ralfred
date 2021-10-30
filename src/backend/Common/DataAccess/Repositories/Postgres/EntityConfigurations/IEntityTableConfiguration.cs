using LinqToDB.Mapping;


namespace Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfigurations
{
	public interface IEntityTableConfiguration
	{
		void Configure(MappingSchema schema);
	}
}