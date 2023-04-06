using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SS.Identity.API.Configuration;

namespace SS.Identity.API
{
    public class Startup : IStartup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddIdentityConfiguration(Configuration);

            services.AddApiConfiguration();

            services.AddSwaggerConfiguration();
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment) 
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration(environment);
        }
    }

    public interface IStartup 
    {
        IConfiguration Configuration { get; }

        void Configure(WebApplication app, IWebHostEnvironment environment);

        void ConfigureServices(IServiceCollection services);
    }

    public static class StartupExtensions 
    {
        public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder WebAppBuilder) where TStartup : IStartup
        {
            //var startup = Activator.CreateInstance(typeof(TStartup), WebAppBuilder.Configuration) as IStartup;
            var startup = Activator.CreateInstance(typeof(TStartup), WebAppBuilder.Environment) as IStartup;
            if (startup == null) throw new ArgumentException("Invalid Startup.cs class");

            startup.ConfigureServices(WebAppBuilder.Services);

            var app = WebAppBuilder.Build();
            startup.Configure(app, app.Environment);

            app.Run();

            return WebAppBuilder;
        }
    }
}
