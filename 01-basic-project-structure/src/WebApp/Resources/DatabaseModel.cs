using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources
{
    public class DatabaseModel
    {
        private readonly string connectionString;
        private SqlStatementInterceptor _sqlStatementInterceptor;

        public DatabaseModel(SqlStatementInterceptor sqlStatementInterceptor)
        {
            connectionString = "Server=.; Database=NHibernatePractice; Integrated Security=SSPI;";
            _sqlStatementInterceptor = sqlStatementInterceptor;
        }
        
        public ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
            

                .Database(MsSqlConfiguration.MsSql2012

                    .ConnectionString(connectionString).ShowSql()

                )
                .Mappings(m =>

                    m.FluentMappings

                        .AddFromAssemblyOf<Users>())

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