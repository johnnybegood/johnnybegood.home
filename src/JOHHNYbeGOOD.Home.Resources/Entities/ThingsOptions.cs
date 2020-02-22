using System;
using System.Linq;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Resources.Entities
{
    public class ThingsOptions
    {
        public ThingOptions[] Things { get; set; }

        public ThingsOptions AddThing(string id, IDevice device)
        {
            var thing = new ThingOptions
            {
                Id = id,
                Device = device
            };

            Things = Things.Concat(new[] { thing }).ToArray();

            return this;
        }
    }
}
