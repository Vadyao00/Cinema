using Cinema.Application.Services;
using Cinema.LoggerService;
using Cinema.Persistence;
using Contracts.IRepositories;
using Contracts.IServices;
using Microsoft.EntityFrameworkCore;

namespace Cinema.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyHeader().
                AllowAnyOrigin().
                AllowAnyMethod().
                WithExposedHeaders("X-Pagination"));
            });

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<CinemaContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnectionLocal"), b =>
                {
                    b.EnableRetryOnFailure();
                })
            );

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();


    }
}