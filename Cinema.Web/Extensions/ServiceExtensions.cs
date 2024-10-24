﻿using Cinema.Persistence;
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
                AllowAnyMethod());
            });

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<CinemaContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnectionLocal"), b =>
                {
                    b.EnableRetryOnFailure();
                })
            );

        //public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        //    services.AddScoped<IrepositoryManager, RepositoryManager>();
    }
}
