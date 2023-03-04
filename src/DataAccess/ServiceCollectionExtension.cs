using DataAccess.Repositories;
using DataAccess.Repositories.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace DataAccess
{
    public static class ServiceCollectionExtension
    {
        private static string? _connectionString;

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDbConnection>(_ => new Npgsql.NpgsqlConnection(GetConnectionString()));
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
        internal static string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "127.0.0.1";
                var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"; 
                var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "postgres";
                var userId = Environment.GetEnvironmentVariable("DB_USER_ID") ?? "postgres";
                var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

                var connectionString = $"Server={host};Port={port};Database={dbName};User Id={userId};Password={password};";

                _connectionString = connectionString;
            }

            return _connectionString;
        }
    }
}
