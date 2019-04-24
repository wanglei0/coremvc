using System;

namespace WebApp.Resources.Repository.Models
{
    public class UserInfo
    {
        public virtual UserInfo Set(Guid userid, string email, string address)
        {
            Id = Guid.NewGuid();
            user_id = userid;
            Email = email;
            Address = address;
            return this;
        }
        public virtual Guid Id { get; set; }
        public virtual Guid user_id { get; set; }
        public virtual string Email { get; set; }
        public virtual string Address { get; set; }
    }
}