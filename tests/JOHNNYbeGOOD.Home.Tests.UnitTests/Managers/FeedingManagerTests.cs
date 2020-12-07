using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.FeedingManager;
using JOHNNYbeGOOD.Home.Model.Devices;
using JOHNNYbeGOOD.Home.Resources;
using JOHNNYbeGOOD.Home.Tests.UnitTests.Helpers;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace JOHNNYbeGOOD.Home.Tests.UnitTests.Managers
{
    public class FeedingManagerTests
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        [Fact]
        public void GetsNextSlotForSingleClosedSlot()
        {
            var sensor = _mocker.GetMock<IDigitalSensor>();
            var slot = _mocker.GetSlotWithSensor("dummy", sensor);

            sensor.Setup(s => s.Read())
                .Returns(true)
                .Verifiable();

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();
            manager.Slots = new[] { slot };

            var result = manager.NextFeedingSlot();

            sensor.VerifyAll();
            Assert.Equal(slot, result);
        }

        [Fact]
        public void GetsNullNextSlotForSingleOpenSlot()
        {
            var mocker = new AutoMocker();
            var sensor = mocker.GetMock<IDigitalSensor>();

            var slot = _mocker.GetSlotWithSensor("dummy", sensor);

            sensor.Setup(s => s.Read())
                .Returns(false)
                .Verifiable();

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();
            manager.Slots = new[] { slot };

            var result = manager.NextFeedingSlot();

            sensor.VerifyAll();
            Assert.Null(result);
        }

        [Fact]
        public async Task FeedingSuccessfullForSingleClosedSlotAsync()
        {
            var mocker = new AutoMocker();
            var sensor = mocker.GetMock<IDigitalSensor>();
            var gate = mocker.GetMock<IGateDevice>();
            var isClosed = true;

            var slot = _mocker.GetSlotWithGateAndSensor("dummy", gate, sensor);

            gate
                .Setup(g => g.OpenGateAsync())
                .Callback(() => { isClosed = false; })
                .Returns(Task.CompletedTask)
                .Verifiable();

            sensor
                .Setup(s => s.Read())
                .Returns(() => { return isClosed; })
                .Verifiable();

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();
            manager.Slots = new[] { slot };

            var result = await manager.TryFeedAsync();

            _mocker.VerifyAll();
            Assert.True(result.Succeeded);
            Assert.Equal(slot.Name, result.SlotUsed);
        }

        [Fact]
        public async Task FeedingFailsForSingleOpenSlotAsync()
        {
            var mocker = new AutoMocker();
            var sensor = mocker.GetMock<IDigitalSensor>();

            var slot = _mocker.GetSlotWithSensor("dummy", sensor);

            sensor.Setup(s => s.Read())
                .Returns(false)
                .Verifiable();

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();
            manager.Slots = new[] { slot };

            var result = await manager.TryFeedAsync();

            sensor.VerifyAll();

            Assert.False(result.Succeeded);
            Assert.Null(result.SlotUsed);
        }

        [Fact]
        public async Task FeedingFailsIfSlotRemainsClosedAsync()
        {
            var mocker = new AutoMocker();
            var sensor = mocker.GetMock<IDigitalSensor>();

            var slot = _mocker.GetSlotWithSensor("dummy", sensor);

            sensor.Setup(s => s.Read())
                .Returns(true)
                .Verifiable();

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();
            manager.Slots = new[] { slot };

            var result = await manager.TryFeedAsync();

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

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();
            manager.Slots = slots;

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

