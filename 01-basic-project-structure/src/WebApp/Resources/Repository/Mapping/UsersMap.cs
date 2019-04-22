
using FluentNHibernate.Mapping;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository.Mapping
{
    public class UsersMap : ClassMap<Users>
    {
        public UsersMap()
        {
            Table("users");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.LastModified).Not.Nullable();
        }
    }
}