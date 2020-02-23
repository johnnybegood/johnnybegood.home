using System.Collections.Generic;
using System.Linq;
using JOHNNYbeGOOD.Home.Model;
using JOHNNYbeGOOD.Home.Model.Devices;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.Configuration;

namespace JOHNNYbeGOOD.Home.FeedingManager
{
    public class FeedingSlot : IFeedingSlot
    {
        private readonly IGateDevice _gate;
        private readonly IDigitalSensor _sensor;
        private readonly FeedingSlot[] _dependendSlots;

        public string Name { get; set; }

        /// <summary>
        /// Default constructor for <see cref="FeedingSlot"/>
        /// </summary>
        /// <param name="slotConfig"></param>
        /// <param name="dependendSlots"></param>
        /// <param name="thingsResource"></param>
        public FeedingSlot(FeedingSlotOptions options, IEnumerable<FeedingSlot> dependendSlots,IThingsResource thingsResource)
        {
            _gate = thingsResource.GetDevice<IGateDevice>(options.GateId);
            _sensor = thingsResource.GetDevice<IDigitalSensor>(options.SensorId);
            _dependendSlots = dependendSlots == null ? new FeedingSlot[0] : dependendSlots.ToArray();

            Name = options.Name;
        }

        /// <summary>
        /// Constructor for <see cref="FeedingSlot"/>
        /// </summary>
        public FeedingSlot(string name, IGateDevice gate, IDigitalSensor sensor, IEnumerable<FeedingSlot> dependendSlots, IThingsResource thingsResource)
        {
            _gate = gate;
            _sensor = sensor;
            _dependendSlots = dependendSlots.ToArray();

            Name = name;
        }

        /// <summary>
        /// Check if the gate is closed
        /// </summary>
        /// <returns></returns>
        public bool SlotClosed()
        {
            return _sensor.Read();
        }

        /// <summary>
        /// Check if the slot can be open based on the sensor and all the dependend slots
        /// </summary>
        /// <returns></returns>
        public bool CanOpen()
        {
            return SlotClosed() && !_dependendSlots.Any(s => s.SlotClosed());
        }

        /// <summary>
        /// Try to open the slot
        /// </summary>
        /// <returns></returns>
        public bool TryOpenGate()
        {
            if (CanOpen())
            {
                _gate.OpenGateAsync();
            }

            return !SlotClosed();
        }
    }
}