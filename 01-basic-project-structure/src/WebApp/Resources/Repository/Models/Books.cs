using System;

namespace WebApp.Resources.Repository.Models
{
    public class Books
    {
        public Books(Guid userId, string name)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            LastModified = DateTime.Now;
        }
        public virtual Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime LastModified { get; set; }
    }
}