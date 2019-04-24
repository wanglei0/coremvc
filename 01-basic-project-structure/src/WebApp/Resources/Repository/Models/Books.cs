using System;

namespace WebApp.Resources.Repository.Models
{
    public class Books
    {
        public virtual Guid Id { get; set; }
        public virtual string user_id { get; set; }
        public virtual string name { get; set; }
        public virtual DateTime last_modified { get; set; }
        public virtual Users User { get; set; }
    }
}