using System;

using DapperExtensions.Sql;

using DbContext = DapperExtensions.DapperExtensions;


namespace Ralfred.Common.DataAccess.Repositories.Postgres
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