using NHibernate;

namespace WebApp.Resources.Providers
{
    public interface IDatabaseSessionProvider
    {
        ISession OpenSession();
    }
}