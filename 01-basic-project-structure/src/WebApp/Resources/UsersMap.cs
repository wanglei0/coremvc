
using FluentNHibernate.Mapping;

namespace WebApp.Resources
{
    public class UsersMap : ClassMap<Users>
    {
        public UsersMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Table("users");
        }
    }
}