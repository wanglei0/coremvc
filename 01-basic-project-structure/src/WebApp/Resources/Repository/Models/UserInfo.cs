using System;

namespace WebApp.Resources.Repository.Models
{
    public class UserInfo
    {
        public UserInfo(Guid userid, string email, string address)
        {
            Id = Guid.NewGuid();
            UserId = userid;
            Email = email;
            Address = address;
        }
        public virtual Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual string Email { get; set; }
        public virtual string Address { get; set; }
    }
}