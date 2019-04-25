using System;

namespace WebApp.Resources.Repository.Models
{
    public class Book
    {
        public virtual Guid Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime LastModifiedTime { get; set; }
        public virtual User User { get; set; }
    }
}