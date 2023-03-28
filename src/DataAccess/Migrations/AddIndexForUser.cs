using FluentMigrator;

namespace DataAccess.Migrations
{
    [Migration(20230328121400)]
    public class AddIndexForUser : Migration
    {

        public override void Down()
        {
            Execute.Sql("DROP INDEX app_user_search_name_idx;");
        }

        public override void Up()
        {
            Execute.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm;");
            Execute.Sql("CREATE INDEX app_user_search_name_idx ON app_user USING gin (last_name gin_trgm_ops, first_name gin_trgm_ops);");
        }
    }
}
