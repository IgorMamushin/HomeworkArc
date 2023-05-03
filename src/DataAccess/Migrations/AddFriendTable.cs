using FluentMigrator;

namespace DataAccess.Migrations
{
    [Migration(20230503160100)]
    public class AddFriendTable : Migration
    {
        public override void Down()
        {
            Delete.Table("friend");
        }

        public override void Up()
        {
            Execute.Sql("alter table app_user add primary key (id)");

            Create
                .Table("friend")
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("friend_id").AsGuid().NotNullable();

            Create.Index()
                .OnTable("friend")
                .OnColumn("user_id");

            Create.Index()
                .OnTable("friend")
                .OnColumn("friend_id");

            Create
                .UniqueConstraint()
                .OnTable("friend")
                .Columns("user_id", "friend_id");

            Create
                .ForeignKey("FK_user_friend_user")
                .FromTable("friend").ForeignColumn("user_id")
                .ToTable("app_user").PrimaryColumn("id");

            Create
                .ForeignKey("FK_user_friend_friend")
                .FromTable("friend").ForeignColumn("friend_id")
                .ToTable("app_user").PrimaryColumn("id");
        }
    }
}
