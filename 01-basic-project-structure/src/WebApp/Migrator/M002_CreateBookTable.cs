using FluentMigrator;

namespace WebApp.Migrator
{
    [Migration(002)]
    public class M002_CreateBookTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("Book")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("LastModifiedTime").AsDateTime().NotNullable();

        }

        public override void Down()
        {
            Delete.Table("Book");
        }
    }
}
