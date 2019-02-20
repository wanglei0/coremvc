using FluentMigrator;

namespace Migration
{
    [Migration(1)]
    public class CreateMessageTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("message")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("text").AsString(255).NotNullable();
        }

        public override void Down() { Delete.Table("message"); }
    }
}