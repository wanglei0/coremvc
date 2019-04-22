using Microsoft.Extensions.Logging;
using NHibernate;

namespace WebApp.Resources
{
    public class SqlStatementInterceptor : EmptyInterceptor, IInterceptor
    {
        public NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql, ILogger<SqlStatementInterceptor> logger)
        {
            logger.Log(LogLevel.Error, "ERRRRRRRRRRRRRRRRRRRRRRRRROR!");
            logger.Log(LogLevel.Error, sql.ToString());
            SQLString.NHibernateSQLString = sql.ToString();
            return sql;
        }
    }
}