using FluentNHibernate.Mapping;
using WebApp.Resources.Repository.Models;

namespace WebApp.Resources.Repository.Mapping
{
    public class BooksMap : ClassMap<Books>
    {
        public BooksMap()
        {
            Table("users");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.LastModified).Not.Nullable();
            
//            HasMany<Books>(h => h.Id).LazyLoad().AsSet().KeyColumn("UserId").Cascade.All().Inverse();
        }
    }
}