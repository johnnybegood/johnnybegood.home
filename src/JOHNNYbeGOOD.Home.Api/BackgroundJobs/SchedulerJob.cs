using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOHNNYbeGOOD.Home.Api.BackgroundJobs
{
    public class SchedulerJob : BackgroundService
    {
        private const int RunDelay = 10000;
        private readonly ILogger<SchedulerJob> _logger;
        private readonly IServiceProvider _services;

        /// <summary>
        /// Default constructor for <see cref="SchedulerJob"/>
        /// </summary>
        /// <param name="logger"></param>
        public SchedulerJob(ILogger<SchedulerJob> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting scheduler job");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(RunDelay, stoppingToken);

                using var scope = _services.CreateScope();
                var feedingManager = scope.ServiceProvider.GetRequiredService<IFeedingManager>();

                _logger.LogDebug("Checking schedule");

                var result = await feedingManager.TryScheduledFeedAsync();
            }
        }
    }
}
