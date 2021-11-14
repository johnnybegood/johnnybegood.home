using System;
using System.Collections.Generic;
using System.Linq;

namespace JOHNNYbeGOOD.Home.FeedingManager
{
    public class FeedingManagerOptions
    {
        public ICollection<FeedingSlotOptions> FeedingSlots { get; } = new List<FeedingSlotOptions>();

        /// <summary>
        /// Add a slot to the feeding manager
        /// </summary>
        /// <param name="name">Name of the slot</param>
        /// <param name="flap">Id of the gate</param>
        /// <param name="sensor">Id of the sensor</param>
        /// <returns></returns>
        public FeedingManagerOptions AddSlot(string name, string flap, string sensor)
        {
            FeedingSlots.Add(new FeedingSlotOptions
            {
                Name = name,
                FlapId = flap,
                SensorId = sensor,
            });

            return this;
        }

        /// <summary>
        /// Add a slot to the feeding manager without sensor
        /// </summary>
        /// <param name="name">Name of the slot</param>
        /// <param name="flap">Id of the gate</param>
        /// <param name="sensor">Id of the sensor</param>
        /// <returns></returns>
        public FeedingManagerOptions AddUncheckedSlot(string name, string flap)
        {
            FeedingSlots.Add(new FeedingSlotOptions
            {
                Name = name,
                FlapId = flap,
                BypassSensor = true,
                SensorId = null
            }) ;

            return this;
        }

        public FeedingManagerOptions AddSlot(FeedingSlotOptions options)
        {
            FeedingSlots.Add(options);

            return this;
        }
    }
}
