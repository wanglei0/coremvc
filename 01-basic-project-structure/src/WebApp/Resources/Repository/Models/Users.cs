using System;

namespace WebApp.Resources.Repository.Models
{
    public class Users
    {
        public Users()
        {
            Id = Guid.NewGuid();
            FirstName = "firstName";
            LastName = "lastName";
            LastModified = DateTime.Now;
        }
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime LastModified { get; set; }
    }
}