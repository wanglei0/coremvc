using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebApp.Resources.Repository.Models
{
    public class Users
    {
        public Users()
        {
//            Id = Guid.NewGuid();
//            FirstName = firstName;
//            LastName = lastName;
//            LastModified = DateTime.Now;
        }
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime LastModified { get; set; }
        public virtual ICollection<Books> Books { get; set; }

        public virtual Users Set(string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            LastModified = DateTime.Now;
//            Books = new Collection<Books>();
            return this;
        }
    }
}