using JOHHNYbeGOOD.Home.Engines.Hosting;
using JOHHNYbeGOOD.Home.FeedingManager.Hosting;
using JOHHNYbeGOOD.Home.Resources.Devices;
using JOHHNYbeGOOD.Home.Resources.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace JOHNNYbeGOOD.Home.Api
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
            services.AddControllers();

            services.AddRpiThings(o => o
                .AddThing("gate-1", () => new DockerPiRelayChannelDevice(1, 0x10, 0x01)));
                //.AddThing("gate-2", () => new DockerPiRelayChannelDevice(1, 0x10, 0x01))
                //.AddThing("sensor-1", () => new RpiInputPinDevice(26))
                //.AddThing("sensor-2", () => new RpiInputPinDevice(13)));

            services.AddFeedingManager(o => o
                .AddUncheckedSlot("slot-1", "gate-1")
                //.AddSlot("slot-2", "gate-2", "sensor-2")
            );

            services.AddDefaultScheduling();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("202002", new OpenApiInfo { Title = "Feeder internal API", Version = "202002" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/202002/swagger.json", "Feeder internal API - version 202002");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
