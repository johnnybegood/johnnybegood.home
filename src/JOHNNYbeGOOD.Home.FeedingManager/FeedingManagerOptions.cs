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
        /// <param name="gate">Id of the gate</param>
        /// <param name="sensor">Id of the sensor</param>
        /// <returns></returns>
        public FeedingManagerOptions AddSlot(string name, string gate, string sensor)
        {
            FeedingSlots.Add(new FeedingSlotOptions
            {
                Name = name,
                GateId = gate,
                SensorId = sensor
            });

            return this;
        }

        public FeedingManagerOptions AddSlot(FeedingSlotOptions options)
        {
            FeedingSlots.Add(options);

            return this;
        }
    }
}
