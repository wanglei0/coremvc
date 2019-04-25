using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources
{
    public static class FluentNHibernateHelper

    {

        public static ISession OpenSession()

        {

            string connectionString = "Server=.; Database=NHibernatePractice; Integrated Security=SSPI;";

            ISessionFactory sessionFactory = Fluently.Configure()

                .Database(MsSqlConfiguration.MsSql2012

                    .ConnectionString(connectionString).ShowSql()

                )

                .Mappings(m =>

                    m.FluentMappings

                        .AddFromAssemblyOf<User>())

                .ExposeConfiguration(cfg => new SchemaExport(cfg)

                    .Create(false, false))

                .BuildSessionFactory();

            return sessionFactory.OpenSession();

        }

    }
}