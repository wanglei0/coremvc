using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrator
{
    [Migration("1")]
    public class M001_Create_User_Table : FluentMigrator.Migration
    {

        public override void Up()
        {
            Create.Table("Userssss")
                .WithColumn("Type").AsString().PrimaryKey()
                .WithColumn("CurrentPage").AsString().NotNullable()
                .WithColumn("LastModifiedTime").AsDateTime().NotNullable();

        }

        public override void Down()
        {
            Delete.Table("Userssss");
        }
    }
}