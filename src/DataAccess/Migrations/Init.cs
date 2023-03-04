using FluentMigrator;

namespace DataAccess.Migrations
{
    [Migration(20230304190000)]
    public class Init : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql("CREATE EXTENSION if not exists \"uuid-ossp\"");
            Execute.Sql("CREATE EXTENSION if not exists \"pgcrypto\"");

            Create.Table("app_user")
                .WithColumn("id").AsGuid()
                .WithColumn("first_name").AsString(50)
                .WithColumn("last_name").AsString(50)
                .WithColumn("age").AsInt32()
                .WithColumn("biography").AsString(1000)
                .WithColumn("city").AsString(30)
                .WithColumn("password_hash").AsString(100);

        }
    }
}
