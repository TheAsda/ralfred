using System;

using DapperExtensions.Sql;

using DbContext = DapperExtensions.DapperExtensions;


namespace Ralfred.Common.DataAccess.Repositories.InMemory
{
	public abstract class BasePostgresRepository
	{
		protected BasePostgresRepository(Type mapperType)
		{
			DbContext.DefaultMapper = mapperType;
			DbContext.SqlDialect = new PostgreSqlDialect();
		}
	}
}