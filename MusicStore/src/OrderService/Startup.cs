using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Models;
using Steeltoe.CloudFoundry.Connector.SqlServer.EFCore;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.CloudFoundry;

namespace OrderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add managment endpoint services
            services.AddCloudFoundryActuators(Configuration);

            // Add framework services.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add framework services.
            services.AddMvc();

            services.AddDiscoveryClient(Configuration);

            services.AddDbContext<OrdersContext>(options => options.UseSqlServer(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            // Add management endpoints into pipeline
            app.UseCloudFoundryActuators();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseDiscoveryClient();
        }
    }
}
