using LinqToDB.Mapping;


namespace Ralfred.Common.DataAccess.Repositories.Postgres.EntityConfiguration
{
	public interface IMapper
	{
		void Apply(MappingSchema schema);
	}
}