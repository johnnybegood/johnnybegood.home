using System.Device.Gpio;
using System.Device.I2c;
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

            services.AddSingleton(I2cDevice.Create(new I2cConnectionSettings(1, 0x10)));

            var i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x10));
            var controller = new GpioController(PinNumberingScheme.Logical);

            //Register i2c so that it gets disposed
            services.AddSingleton(i2c);

            services.AddRpiThings(o => o
                .AddThing("gate-1", new DockerPiRelayChannelDevice(0x01, i2c))
                .AddThing("gate-2", new DockerPiRelayChannelDevice(0x01, i2c))
                .AddThing("sensor-1", new RpiInputPinDevice(12, controller))
                .AddThing("sensor-1", new RpiInputPinDevice(12, controller)));

            services.AddFeedingManager(o => o
                .AddSlot("slot-1", "gate-1", "sensor-1")
                .AddSlot("slot-2", "gate-2", "sensor-2")
            );

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
            }

            app.UseHttpsRedirection();

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
            });
        }
    }
}
