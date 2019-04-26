using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources
{
    public class DatabaseModel
    {
        private readonly string connectionString;
        private SqlStatementInterceptor _sqlStatementInterceptor;
        private TableNameConvention _tableNameConvention;

        public DatabaseModel(SqlStatementInterceptor sqlStatementInterceptor, IConfiguration config, TableNameConvention tableNameConvention)
        {
            connectionString = "Server=.; Database=NHibernatePractice; Integrated Security=SSPI;";
            _sqlStatementInterceptor = sqlStatementInterceptor;
            _tableNameConvention = tableNameConvention;
        }
        
        public ISessionFactory CreateSessionFactory()
        {
            
            return Fluently.Configure()
            

                .Database(MsSqlConfiguration.MsSql2012

                    .ConnectionString(connectionString).ShowSql().FormatSql()

                )
                .Mappings(m =>

                    m.FluentMappings.AddFromAssemblyOf<User>()
                        .Conventions.Add(_tableNameConvention))
                
                .ExposeConfiguration(cfg => new SchemaExport(cfg)

                    .Create(false, false))
                
//                .ExposeConfiguration(x =>
//                {
//                    x.SetInterceptor(_sqlStatementInterceptor);
//                })

                .BuildSessionFactory();
        }
    }
}