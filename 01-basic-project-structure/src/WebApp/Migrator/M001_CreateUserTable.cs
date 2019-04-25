using FluentMigrator;

namespace WebApp.Migrator
{
    [Migration(001)]
    public class M001_CreateUserTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("LastModifiedTime").AsDateTime().NotNullable();

        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}