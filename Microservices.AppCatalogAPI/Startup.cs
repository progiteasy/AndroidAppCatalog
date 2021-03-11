using MicroservicesCommonData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microservices.AppCatalogAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Settings.AppCatalogDbConnectionString = configuration["AppCatalogDbConnectionString"];
            Settings.AppDescriptionWriterServiceAddress = configuration["AppDescriptionWriterServiceAddress"];
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<AppCatalogDbContext>(options => options.UseNpgsql(Settings.AppCatalogDbConnectionString));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}