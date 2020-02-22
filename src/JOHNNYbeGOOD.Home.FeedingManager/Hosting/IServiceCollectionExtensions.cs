using System;
using JOHNNYbeGOOD.Home;
using JOHNNYbeGOOD.Home.FeedingManager;
using Microsoft.Extensions.DependencyInjection;

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
            serviceCollection.AddTransient<IFeedingManager, DefaultFeedingManager>();

            return serviceCollection;
        }
    }
}
