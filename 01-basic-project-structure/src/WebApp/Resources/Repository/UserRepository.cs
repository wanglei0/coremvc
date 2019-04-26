using System;
using System.Linq;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using WebApp.Resources.Providers;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository
{
//    public interface IUsersRepository
//    {
//        Guid Save();
//        Users Get(Guid id);
//        void Delete<Users>(Guid id);
//    }
    public class UserRepository : BaseRepository<User>
    {
//        IDatabaseSessionProvider DatabaseSessionProvider { get; }
        
        public UserRepository(IDatabaseSessionProvider databaseSessionProvider) : base(databaseSessionProvider)
        {
//            DatabaseSessionProvider = databaseSessionProvider;
        }

//        public Guid Save()
//        {
//            var user = new Users();
//            using (var session = DatabaseSessionProvider.OpenSession())
//            {
//                session.Save(user);
//                session.Flush();
//            }
//
//            return user.Id;
//        }
//        
//        public Users Get(Guid id)
//        {
//            using (var session = DatabaseSessionProvider.OpenSession())
//            {
//                return session 
//                    .Query<Users>().First(c => c.Id == id);
//            }
//        }
//        
//        public void Delete<Users>(Guid id)
//        {
//            using (var session = DatabaseSessionProvider.OpenSession())
//            {
//                var queryString = string.Format("DELETE {0} where id = :id",
//                    typeof(Users));
//                session.CreateQuery(queryString)
//                    .SetParameter("id", id)
//                    .ExecuteUpdate();
//                session.Flush();
//            }
//        }
    }
}