using System;
using System.Collections.Generic;
using System.Linq;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.FeedingManager;
using JOHNNYbeGOOD.Home.Model.Devices;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace JOHNNYbeGOOD.Home.Tests.UnitTests.Managers
{
    public class FeedingManagerTests
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        public FeedingManagerTests()
        {
        }

        [Fact]
        public void GetsNextSlotForSingleClosedSlot()
        {
            var sensor = _mocker.GetMock<IDigitalSensor>();

            var slot = new FeedingSlot("dummy",
                _mocker.Get<IGateDevice>(),
                sensor.Object,
                Enumerable.Empty<FeedingSlot>(),
                _mocker.Get<IThingsResource>());

            sensor.Setup(s => s.Read())
                .Returns(true)
                .Verifiable();

            var manager = new DefaultFeedingManager(new[] { slot },
                _mocker.Get<ILogger<DefaultFeedingManager>>(),
                _mocker.Get<ISchedulingEngine>());

            var result = manager.NextFeedingSlot();

            sensor.VerifyAll();
            Assert.Equal(slot, result);
        }

        [Fact]
        public void GetsNullNextSlotForSingleOpenSlot()
        {
            var mocker = new AutoMocker();
            var sensor = mocker.GetMock<IDigitalSensor>();

            var slot = new FeedingSlot("dummy",
                mocker.Get<IGateDevice>(),
                sensor.Object,
                Enumerable.Empty<FeedingSlot>(),
                mocker.Get<IThingsResource>());

            sensor.Setup(s => s.Read())
                .Returns(false)
                .Verifiable();

            var manager = new DefaultFeedingManager(new[] { slot },
                mocker.Get<ILogger<DefaultFeedingManager>>(),
                mocker.Get<ISchedulingEngine>());

            var result = manager.NextFeedingSlot();

            sensor.VerifyAll();
            Assert.Null(result);
        }

        [Fact]
        public void FeedingSuccessfullForSingleClosedSlot()
        {
            var mocker = new AutoMocker();
            var sensor = mocker.GetMock<IDigitalSensor>();
            var gate = mocker.GetMock<IGateDevice>();
            var isClosed = true;

            var slot = new FeedingSlot("dummy",
                gate.Object,
                sensor.Object,
                Enumerable.Empty<FeedingSlot>(),
                mocker.Get<IThingsResource>());

            gate.Setup(g => g.OpenGate()).Callback(() => { isClosed = false; }).Verifiable();
            sensor.Setup(s => s.Read()).Returns(() => { return isClosed; }).Verifiable();

            var manager = new DefaultFeedingManager(new[] { slot },
                mocker.Get<ILogger<DefaultFeedingManager>>(),
                mocker.Get<ISchedulingEngine>());

            var result = manager.TryFeed();

            _mocker.VerifyAll();
            Assert.True(result.Succeeded);
            Assert.Equal(slot.Name, result.SlotUsed);
        }

        [Fact]
        public void FeedingFailsForSingleOpenSlot()
        {
            var mocker = new AutoMocker();
            var sensor = mocker.GetMock<IDigitalSensor>();

            var slot = new FeedingSlot("dummy",
                mocker.Get<IGateDevice>(),
                sensor.Object,
                Enumerable.Empty<FeedingSlot>(),
                mocker.Get<IThingsResource>());

            sensor.Setup(s => s.Read())
                .Returns(false)
                .Verifiable();

            var manager = new DefaultFeedingManager(new[] { slot },
                mocker.Get<ILogger<DefaultFeedingManager>>(),
                mocker.Get<ISchedulingEngine>());

            var result = manager.TryFeed();

            sensor.VerifyAll();

            Assert.False(result.Succeeded);
            Assert.Null(result.SlotUsed);
        }

        [Fact]
        public void FeedingFailsIfSlotRemainsClosed()
        {
            var mocker = new AutoMocker();
            var sensor = mocker.GetMock<IDigitalSensor>();

            var slot = new FeedingSlot("dummy",
                mocker.Get<IGateDevice>(),
                sensor.Object,
                Enumerable.Empty<FeedingSlot>(),
                mocker.Get<IThingsResource>());

            sensor.Setup(s => s.Read())
                .Returns(true)
                .Verifiable();

            var manager = new DefaultFeedingManager(new[] { slot },
                mocker.Get<ILogger<DefaultFeedingManager>>(),
                mocker.Get<ISchedulingEngine>());

            var result = manager.TryFeed();

            sensor.VerifyAll();

            Assert.False(result.Succeeded);
        }

        [Theory]
        [InlineData(new[] { true, true, true }, 0)]
        [InlineData(new[] { false, true, true }, 1)]
        [InlineData(new[] { true, false, true }, 0)]
        [InlineData(new[] { false, false, true }, 2)]
        [InlineData(new[] { false, false, false }, null)]
        public void CorrectSlotTheory(bool[] slotsClosed, int? nextSlot)
        {
            var sensors = new List<IDigitalSensor>();
            var slots = new List<FeedingSlot>();

            for (int i = 0; i < slotsClosed.Length; i++)
            {
                var sensor = new Mock<IDigitalSensor>();
                sensor.Setup(s => s.Read()).Returns(slotsClosed[i]).Verifiable();
                sensors.Add(sensor.Object);

                slots.Add(new FeedingSlot(i.ToString(),
                    _mocker.Get<IGateDevice>(),
                    sensor.Object,
                    slots,
                    _mocker.Get<IThingsResource>()));
            }

            var manager = new DefaultFeedingManager(slots,
                _mocker.Get<ILogger<DefaultFeedingManager>>(),
                _mocker.Get<ISchedulingEngine>());

            var result = manager.NextFeedingSlot();

            _mocker.VerifyAll();

            if (nextSlot.HasValue)
            {
                Assert.Equal(nextSlot.Value.ToString(), result.Name);
            }
            else
            {
                Assert.Null(result);
            }
        }
    }
}

