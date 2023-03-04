using DataAccess;

namespace WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(setupAction =>
            {
                setupAction.AddPolicy("MyPolicy",
                    builder =>
                    {
                        builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddRepositories(_configuration);
        }

        public void Configure(WebApplication app, IWebHostEnvironment env) 
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.MapDefaultControllerRoute();
        }
    }
}
