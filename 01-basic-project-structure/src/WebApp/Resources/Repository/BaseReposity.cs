using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using WebApp.Resources.Providers;

namespace WebApp.Resources.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        protected IDatabaseSessionProvider DatabaseSessionProvider { get; set; }

        public BaseRepository(IDatabaseSessionProvider databaseSessionProvider)
        {
            DatabaseSessionProvider = databaseSessionProvider;
        }

        public List<T> GetAll()
        {
            using (ISession Session = DatabaseSessionProvider.OpenSession())
            {
                return Session.Query<T>().ToList();
            }
        }

        public T GetById(Guid id)
        {
            using (ISession Session = DatabaseSessionProvider.OpenSession())
            {
                return Session.Get<T>(id);
            }
        }

        public T Insert(T entity)
        {
            try
            {
                using (ISession Session = DatabaseSessionProvider.OpenSession())
                {
                    using (ITransaction Transaction = Session.BeginTransaction())
                    {
                        Session.Save(entity);
                        Transaction.Commit();
                    }
                }

                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(T entity)
        {
            using (ISession Session = DatabaseSessionProvider.OpenSession())
            {
                using (ITransaction Transaction = Session.BeginTransaction())
                {
                    Session.Update(entity);
                    Transaction.Commit();
                }
            }
        }

        public void Delete(Guid id)
        {
            using (ISession Session = DatabaseSessionProvider.OpenSession())
            {
                using (ITransaction Transaction = Session.BeginTransaction())
                {
                    Session.Delete(Session.Load<T>(id));
                    Transaction.Commit();
                }
            }
        }
    }
}