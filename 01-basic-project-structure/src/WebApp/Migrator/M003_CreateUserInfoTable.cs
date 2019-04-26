using FluentMigrator;

namespace WebApp.Migrator
{
    [Migration(003)]
    public class M003_CreateUserInfoTable : Migration
    {
        public override void Up()
        {
            Create.Table("UserInfo")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Address").AsDateTime().NotNullable();

        }

        public override void Down()
        {
            Delete.Table("UserInfo");
        }
    }
}