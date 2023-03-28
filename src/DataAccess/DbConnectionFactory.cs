using System.Data;

namespace DataAccess
{
    internal class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            return new Npgsql.NpgsqlConnection(ConnectionStringBuilder.GetRwConnectionString());
        }
    }
}
