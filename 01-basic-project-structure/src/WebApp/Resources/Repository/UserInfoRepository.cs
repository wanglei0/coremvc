using System;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository
{
    public interface IUserInfoRepository
    {
        Guid Save(Guid userid, string email, string address);
        UserInfo Get(Guid id);
        void Delete<UserInfo>(Guid id);
    }
    
    public class UserInfoRepository
    {
        
    }
}