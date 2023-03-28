using DataAccess.Repositories;
using DataAccess.Repositories.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace DataAccess
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }        
    }
}
