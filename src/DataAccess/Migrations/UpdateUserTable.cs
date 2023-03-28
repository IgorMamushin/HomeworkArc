using FluentMigrator;

namespace DataAccess.Migrations
{
    [Migration(20230320145000)]
    public class UpdateUserTable  : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter
                .Table("app_user")
                .AlterColumn("city").AsString(100)
                .AlterColumn("biography").AsString(1000).Nullable();
        }
    }
}
