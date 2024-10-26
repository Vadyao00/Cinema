using Cinema.API.Data;
using Cinema.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinema.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services,builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                //app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHsts();
            }

            ConfigureApp(app);

            app.Map("/", () => "Course work");
            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureCors();

            services.ConfigureSqlContext(configuration);

            services.ConfigureRepositoryManager();

            services.ConfigureServiceManager();

        }

        public static void ConfigureApp(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
            });
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

        }
    }
}
