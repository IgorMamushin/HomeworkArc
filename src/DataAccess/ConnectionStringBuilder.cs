namespace DataAccess
{
    internal class ConnectionStringBuilder
    {
        private static string? _connectionString;

        internal static string GetRwConnectionString(int timeout = 2)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "127.0.0.1";
                var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"; 
                var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "postgres";
                var userId = Environment.GetEnvironmentVariable("DB_USER_ID") ?? "postgres";
                var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

                var connectionString = $"Server={host};Port={port};Database={dbName};User Id={userId};Password={password};Maximum Pool Size=200;ConnectionIdleLifetime=5;ConnectionPruningInterval=5;Command Timeout={timeout};";

                _connectionString = connectionString;
            }

            return _connectionString;
        }
    }
}
