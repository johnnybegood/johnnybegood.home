using System;
using JOHHNYbeGOOD.Home.Resources.Connectors;
using JOHHNYbeGOOD.Home.Resources.Devices;
using JOHHNYbeGOOD.Home.Resources.Entities;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace JOHHNYbeGOOD.Home.Resources.Hosting
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="RPiThingsResource"/> and config to the <paramref name="serviceCollection"/>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configureThings"></param>
        /// <returns></returns>
        public static IServiceCollection AddRpiThings(this IServiceCollection serviceCollection, Action<ThingsOptions> configureThings)
        {
            serviceCollection.Configure(configureThings);
            serviceCollection.AddSingleton<IRpiConnectionFactory, RpiConnectionFactory>();
            serviceCollection.AddTransient<IThingsResource, RPiThingsResource>();

            return serviceCollection;
        }
    }
}
