using System;
using System.Collections.Generic;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Resources.Entities
{
    public class ThingOptions
    {
        public string Id { get; set; }

        public Func<IDevice> Device { get; set; }

        public IDictionary<string, Type> Dependencies { get; set; }
    }
}