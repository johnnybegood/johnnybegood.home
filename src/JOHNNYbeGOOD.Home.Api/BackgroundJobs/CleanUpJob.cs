using System;
using System.Threading;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOHNNYbeGOOD.Home.Api.BackgroundJobs
{
    /// <summary>
    /// Clean up job
    /// </summary>
    public class CleanUpJob : BackgroundService
    {
        private const int RunDelay = 86400000; //24hours

        private readonly ILogger<SchedulerJob> _logger;
        private readonly IServiceProvider _services;

        public CleanUpJob(ILogger<SchedulerJob> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _services = serviceProvider;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting clean up job");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();
                var resource = scope.ServiceProvider.GetRequiredService<IScheduleResource>();

                _logger.LogInformation("Clean up database");
                await resource.CleanUpLog();

                await Task.Delay(RunDelay, stoppingToken);
            }
        }
    }
}

