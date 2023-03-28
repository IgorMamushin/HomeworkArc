using Dapper;
using FluentMigrator;
using System.Reflection;
using System.Text;

namespace DataAccess.Migrations
{
    [Migration(20230320170900)]
    public class AddUsers : Migration
    {
        public override void Down()
        {
            Delete
                .FromTable("app_user")
                .AllRows();
        }

        public override void Up()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DataAccess.people.csv";

            var sb = new StringBuilder();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string? line = reader.ReadLine();
                var count = 0;

                sb.Append("INSERT INTO app_user (id, first_name, last_name, age, city, password_hash) VALUES ");

                while (line != null)
                {
                    var values = line.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length >= 3)
                    {
                        var name = values[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                        if (name.Length >= 2)
                        {
                            if(count > 0)
                            {
                                sb.Append(",");
                            }

                            sb.AppendLine($"('{Guid.NewGuid()}', '{name[1]}', '{name[0]}', {values[1]}, '{values[2]}', crypt('123', gen_salt('md5')))");
                            count++;
                        }
                    }

                    if (count >= 1000)
                    {
                        sb.Append(";");
                        Save(sb.ToString());
                        sb.Clear();
                        sb.Append("INSERT INTO app_user (id, first_name, last_name, age, city, password_hash) VALUES ");
                        count = 0;
                    }

                    line = reader.ReadLine();
                }

                if (count > 0)
                {
                    sb.Append(";");
                    Save(sb.ToString());
                }
            }
        }

        private void Save(string sql)
        {
            using var dbConnection = new Npgsql.NpgsqlConnection()
            {
                ConnectionString = ConnectionString
            };

            dbConnection.Open();

            var trans = dbConnection.BeginTransaction();

            dbConnection.ExecuteScalar(sql);

            trans.Commit();
            dbConnection.Close();
        }
    }
}
