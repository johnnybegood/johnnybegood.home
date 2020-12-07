using System;
using System.Linq;
using JOHNNYbeGOOD.Home.FeedingManager;
using JOHNNYbeGOOD.Home.Model.Devices;
using JOHNNYbeGOOD.Home.Resources;
using Moq;
using Moq.AutoMock;

namespace JOHNNYbeGOOD.Home.Tests.UnitTests.Helpers
{
    public static class MockerExtensions
    {

        /// <summary>
        /// Get <see cref="FeedingSlot"/>
        /// </summary>
        /// <param name="mocker">Instance of <see cref="AutoMocker"/></param>
        /// <param name="name">Name of the slot</param>
        /// <returns></returns>
        public static FeedingSlot GetSlot(this AutoMocker mocker, string name)
        {
            return new FeedingSlot(name,
                        mocker.Get<IGateDevice>(),
                        mocker.Get<IDigitalSensor>(),
                        Enumerable.Empty<FeedingSlot>(),
                        mocker.Get<IThingsResource>());
        }

        /// <summary>
        /// Get <see cref="FeedingSlot"/> 
        /// </summary>
        /// <param name="mocker">Instance of <see cref="AutoMocker"/></param>
        /// <param name="name">Name of the slot</param>
        /// <param name="sensor">The sensor to link with the <see cref="FeedingSlot"/></param>
        /// <returns></returns>
        public static FeedingSlot GetSlotWithSensor(this AutoMocker mocker, string name, Mock<IDigitalSensor> sensor)
        {
            return new FeedingSlot(name,
                        mocker.Get<IGateDevice>(),
                        sensor.Object,
                        Enumerable.Empty<FeedingSlot>(),
                        mocker.Get<IThingsResource>());
        }

        /// <summary>
        /// Get <see cref="FeedingSlot"/> 
        /// </summary>
        /// <param name="mocker">Instance of <see cref="AutoMocker"/></param>
        /// <param name="name">Name of the slot</param>
        /// <param name="sensor">The sensor to link with the <see cref="FeedingSlot"/></param>
        /// <param name="gate">The gate to link with the <see cref="FeedingSlot"/></param>
        /// <returns></returns>
        public static FeedingSlot GetSlotWithGateAndSensor(this AutoMocker mocker, string name, Mock<IGateDevice> gate, Mock<IDigitalSensor> sensor)
        {
            return new FeedingSlot(name,
                        gate.Object,
                        sensor.Object,
                        Enumerable.Empty<FeedingSlot>(),
                        mocker.Get<IThingsResource>());
        }
    }
}
