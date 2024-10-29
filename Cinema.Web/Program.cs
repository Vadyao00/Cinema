using Cinema.API.Data;
using Cinema.API.Extensions;
using Cinema.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using Cinema.Controllers.Filters;

namespace Cinema.API
{
    public class Program
    {
        static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
        new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>().First();

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

            app.MapControllers();

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureCors();

            services.AddScoped<ValidationFilterAttribute>();

            services.ConfigureLoggerService();

            services.ConfigureRepositoryManager();

            services.ConfigureServiceManager();

            services.ConfigureSqlContext(configuration);

            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
            }).AddXmlDataContractSerializerFormatters()
              .AddApplicationPart(typeof(AssemblyReference).Assembly);

            services.AddAutoMapper(typeof(Program));

            services.AddAuthorization();
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