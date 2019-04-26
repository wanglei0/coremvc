using NHibernate;
using NHibernate.Cfg;

namespace WebApp.Resources.Providers
{
    public class DatabaseSessionProvider : IDatabaseSessionProvider
    {
        private ISessionFactory sessionFactory { get; }
        private IInterceptor interceptor;

        public DatabaseSessionProvider(DatabaseModel dbModel, SqlStatementInterceptor sqlStatementInterceptor)
        {
            sessionFactory = dbModel.CreateSessionFactory();
            interceptor = sqlStatementInterceptor;
        }

        public ISession OpenSession()
        {
            var session = sessionFactory.WithOptions().Interceptor(interceptor).FlushMode(FlushMode.Manual).OpenSession();
            return session;
        }
    }
}
