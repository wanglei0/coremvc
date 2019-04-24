
using FluentNHibernate.Mapping;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository.Mapping
{
    public class UsersMap : ClassMap<Users>
    {
        public UsersMap()
        {
            Table("Users");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.LastModified).Not.Nullable();
            
//            HasOne<UserInfo>(u　=>　u.Id).Cascade.All().PropertyRef("UserId");


            HasMany(h => h.Books).Table("books").KeyColumn("user_id").Not.LazyLoad();
//            HasMany(x => x.Books).Not.LazyLoad();
        }
    }
}