using Microservices.AppDescriptionWriter.Services;
using MicroservicesCommonData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microservices.AppDescriptionWriter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Settings.AppCatalogDbConnectionString = configuration["AppCatalogDbConnectionString"];
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddDbContext<AppCatalogDbContext>(options => options.UseNpgsql(Settings.AppCatalogDbConnectionString));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapGrpcService<AppDescriptionWriterService>());
        }
    }
}