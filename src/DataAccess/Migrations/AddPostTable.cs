using FluentMigrator;

namespace DataAccess.Migrations
{
    [Migration(20230503181500)]
    public class AddPostTable : Migration
    {
        public override void Down()
        {
            Delete.Table("post");
        }

        public override void Up()
        {
            Create
                .Table("post")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("text").AsString().NotNullable()
                .WithColumn("user_id").AsGuid().NotNullable()
                .WithColumn("created_at").AsDateTime().NotNullable()
                ;

            Create
                .Index()
                .OnTable("post")
                .OnColumn("user_id");

            Create
                .ForeignKey("FK_user_post")
                .FromTable("post").ForeignColumn("user_id")
                .ToTable("app_user").PrimaryColumn("id");
        }
    }
}
