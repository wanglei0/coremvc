using System;
using System.Linq;
using WebApp.Resources.Providers;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository
{
    public interface IUsersRepository
    {
        Guid Save();
        Users Get(Guid id);
        void Delete<Users>(Guid id);
    }
    public class UsersRepository : IUsersRepository
    {
        IDatabaseSessionProvider DatabaseSessionProvider { get; }
        
        public UsersRepository(IDatabaseSessionProvider dbSessionProvider)
        {
            DatabaseSessionProvider = dbSessionProvider;
        }

        public Guid Save()
        {
            var user = new Users();
            using (var session = DatabaseSessionProvider.OpenSession())
            {
//                var user = session
//                    .Query<Users>()
//                    .FirstOrDefault(u => u.Id == EXISTING.Id);
//
//                if (user != null)
//                {
//                    return;
//                }

                session.Save(user);
                session.Flush();
            }

            return user.Id;
        }
        
        public Users Get(Guid id)
        {
            using (var session = DatabaseSessionProvider.OpenSession())
            {
//                var queryString = string.Format("SELECT {0} where id = :id",
//                    typeof(Users));
                return session 
                    .Query<Users>().First(c => c.Id == id);
            }
        }
        
//        public void Delete(Guid id)
//        {
//            const string query = @"DELETE FROM Users where id = @id";
//
//            using (var session = DatabaseSessionProvider.OpenSession())
//            {
//                session.Query(query + );
//            }
//        }
        
        public void Delete<Users>(Guid id)
        {
            using (var session = DatabaseSessionProvider.OpenSession())
            {
                var queryString = string.Format("DELETE {0} where id = :id",
                    typeof(Users));
                session.CreateQuery(queryString)
                    .SetParameter("id", id)
                    .ExecuteUpdate();
                session.Flush();
            }
            
        }
    }
}