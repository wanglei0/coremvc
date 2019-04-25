using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebApp.Resources.Repository.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime LastModifiedTime { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual IList<Book> Books { get; set; }

        public virtual User Set(string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            LastModifiedTime = DateTime.Now;
            return this;
        }
    }
}