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
                .AddThing("actor-1", () => new DockerPiRelayChannelDevice(1, 0x10, 0x01))
                .AddThing("actor-2", () => new DockerPiRelayChannelDevice(1, 0x10, 0x02))
                .AddThing("actor-3", () => new DockerPiRelayChannelDevice(1, 0x10, 0x03))
                .AddThing("actor-4", () => new DockerPiRelayChannelDevice(1, 0x10, 0x04))
                .AddThing("sensor-1", () => new RpiInputPinDevice(26, isNC: true))
                .AddThing("sensor-2", () => new RpiInputPinDevice(19, isNC: true))
                .AddThing("sensor-3", () => new RpiInputPinDevice(16, isNC: true))
                .AddThing("sensor-4", () => new RpiInputPinDevice(5, isNC: true)));

            // Add bottom to top
            services.AddFeedingManager(o => o
                .AddSlot("slot-4", "actor-4", "sensor-4")
                .AddSlot("slot-3", "actor-3", "sensor-3")
                .AddSlot("slot-2", "actor-2", "sensor-2")
                .AddSlot("slot-1", "actor-1", "sensor-1")
            );

            services.AddDefaultScheduling();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("202002", new OpenApiInfo { Title = "Feeder internal API", Version = "202111" });
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
