using System;
using JOHNNYbeGOOD.Home.Engines;
using Microsoft.Extensions.DependencyInjection;

namespace JOHHNYbeGOOD.Home.Engines.Hosting
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="SchedulingEngine"/> and config to the <paramref name="serviceCollection"/>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configureThings"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultScheduling(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISchedulingEngine, SchedulingEngine>();

            return serviceCollection;
        }
    }
}
