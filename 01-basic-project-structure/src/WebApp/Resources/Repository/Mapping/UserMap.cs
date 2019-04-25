
using FluentNHibernate.Mapping;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("User");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.LastModified).Not.Nullable();
            
            HasOne<UserInfo>(u　=>　u.UserInfo).Cascade.All().PropertyRef(x => x.UserId);


//            HasMany(x => x.Books).Table("books").KeyColumn("user_id").Not.LazyLoad();
            HasMany(x => x.Books).KeyColumn("UserId").Not.LazyLoad();
        }
    }
}