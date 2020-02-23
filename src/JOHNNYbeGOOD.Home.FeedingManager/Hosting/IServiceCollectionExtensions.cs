using System;
using JOHNNYbeGOOD.Home;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.FeedingManager;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JOHHNYbeGOOD.Home.FeedingManager.Hosting
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="RPiThingsResource"/> and config to the <paramref name="serviceCollection"/>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configureThings"></param>
        /// <returns></returns>
        public static IServiceCollection AddFeedingManager(this IServiceCollection serviceCollection, Action<FeedingManagerOptions> configureThings)
        {
            serviceCollection.Configure(configureThings);
            serviceCollection.AddTransient<IFeedingManager, DefaultFeedingManager>(p => new DefaultFeedingManager(
                p.GetRequiredService<IOptionsSnapshot<FeedingManagerOptions>>(),
                p.GetRequiredService<ILogger<DefaultFeedingManager>>(),
                p.GetRequiredService<ISchedulingEngine>(),
                p.GetRequiredService<IThingsResource>()));

            return serviceCollection;
        }
    }
}
