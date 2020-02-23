using System;
using Microsoft.Extensions.DependencyInjection;

namespace JOHHNYbeGOOD.Home.Resources
{
    public class RpiThingsBuilder
    {
        private IServiceCollection _serviceCollection;

        internal RpiThingsBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }
    }
}
