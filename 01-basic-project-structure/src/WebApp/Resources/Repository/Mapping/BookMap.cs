using FluentNHibernate.Mapping;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository.Mapping
{
    public class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            Table("`Book`");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.LastModifiedTime).Not.Nullable();
            
//            References(x => x.User).Column("UserId").Not.LazyLoad();
        }
    }
}