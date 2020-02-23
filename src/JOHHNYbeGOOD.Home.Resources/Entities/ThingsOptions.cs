using System;
using System.Collections.Generic;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Resources.Entities
{
    public class ThingsOptions
    {
        public IDictionary<string, ThingOptions> Things { get; } = new Dictionary<string, ThingOptions>();

        /// <summary>
        /// Add <see cref="ThingOptions"/> for Thing with <paramref name="id"/>
        /// </summary>
        /// <param name="id">The unique id of the thing</param>
        /// <param name="options">The options for the thing</param>
        public ThingsOptions AddThing(string id, ThingOptions options)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id is required", nameof(id));
            }

            if (Things.ContainsKey(id))
            {
                throw new ArgumentException("Id is already defined", nameof(id));
            }

            Things.Add(id, options);

            return this;
        }

        /// <summary>
        /// Add <see cref="ThingOptions"/> for Thing with <paramref name="id"/>
        /// </summary>
        /// <param name="id">The unique id of the thing</param>
        /// <param name="deviceFunc"><see cref="Func{TResult}"/> to create the device</param>
        public ThingsOptions AddThing(string id, Func<IDevice> deviceFunc)
        {
            if (deviceFunc is null)
            {
                throw new ArgumentNullException(nameof(deviceFunc));
            }

            var thing = new ThingOptions
            {
                Id = id,
                Device = deviceFunc
            };

            return AddThing(id, thing);
        }
    }
}
