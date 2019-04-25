using FluentNHibernate.Mapping;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository.Mapping
{
    public class UserInfoMap : ClassMap<UserInfo>
    {
        public UserInfoMap()
        {
            Table("UserInfo");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Email).Not.Nullable();
            Map(x => x.Address).Not.Nullable();
            
//            HasOne<Users>(u　=>　u.UserId).Cascade.All().PropertyRef("Id");
        }
    }
}