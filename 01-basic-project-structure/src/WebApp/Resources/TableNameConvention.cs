using FluentNHibernate.Conventions;

namespace WebApp.Resources
{
    public class TableNameConvention : IClassConvention
    {

        public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
        {
            string typeName = instance.EntityType.Name;

//            instance.Table("[" +instance.EntityType.Name + "123]"); // used to config mapping table name
        }
    }
}
