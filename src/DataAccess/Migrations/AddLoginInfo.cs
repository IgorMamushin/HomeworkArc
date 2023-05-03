using FluentMigrator;

namespace DataAccess.Migrations
{
    [Migration(20230503141000)]
    public class AddLoginInfo : Migration
    {
        public override void Down()
        {
            Delete.Table("login_token");
        }

        public override void Up()
        {
            Create
                .Table("login_token")
                .WithColumn("token").AsGuid().NotNullable()
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("user_name").AsString().NotNullable();

            Create
                .Index()
                .OnTable("login_token")
                .OnColumn("token")
                .Unique();
        }
    }
}
