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
    }
}

