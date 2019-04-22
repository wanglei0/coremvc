using NHibernate;
using NHibernate.Cfg;

namespace WebApp.Resources.Providers
{
    public class DatabaseSessionProvider : IDatabaseSessionProvider
    {
        private ISessionFactory sessionFactory { get; }

        public DatabaseSessionProvider(DatabaseModel dbModel)
        {
            sessionFactory = dbModel.CreateSessionFactory();
        }

        public ISession OpenSession()
        {
            var session = sessionFactory.OpenSession();
            session.FlushMode = FlushMode.Manual;
            return session;
        }
    }
}
