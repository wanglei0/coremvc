using FluentNHibernate.Mapping;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository.Mapping
{
    public class BooksMap : ClassMap<Books>
    {
        public BooksMap()
        {
            Table("Books");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.user_id).Not.Nullable();
            Map(x => x.name).Not.Nullable();
            Map(x => x.last_modified).Not.Nullable();
            
//            References(x => x.User).Column("user_id").Not.LazyLoad();
        }
    }
}