using System;
using WebApp.Resources.Providers;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository
{
//    public interface IBooksRepository
//    {
//        Guid Save(Guid userId, string name);
//        Books Get(Guid id);
//        void Delete<Users>(Guid id);
//    }
    
    public class BooksRepository : BaseRepository<Book>
    {
//        IDatabaseSessionProvider DatabaseSessionProvider { get; }
        
        public BooksRepository(IDatabaseSessionProvider databaseSessionProvider) : base(databaseSessionProvider)
        {
//            DatabaseSessionProvider = databaseSessionProvider;
        }
    }
}