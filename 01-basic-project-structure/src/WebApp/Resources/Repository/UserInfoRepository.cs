using System;
using Microsoft.EntityFrameworkCore.Storage;
using WebApp.Resources.Providers;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository
{
//    public interface IUserInfoRepository
//    {
//        Guid Save(Guid userid, string email, string address);
//        UserInfo Get(Guid id);
//        void Delete<UserInfo>(Guid id);
//    }
    
    public class UserInfoRepository : BaseRepository<UserInfo>
    {
//        IDatabaseSessionProvider DatabaseSessionProvider { get; }
        
        public UserInfoRepository(IDatabaseSessionProvider databaseSessionProvider) : base(databaseSessionProvider)
        {
//            DatabaseSessionProvider = databaseSessionProvider;
        }
    }
}