using System;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.FeedingManager;
using JOHNNYbeGOOD.Home.Model.Devices;
using JOHNNYbeGOOD.Home.Tests.UnitTests.Helpers;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace JOHNNYbeGOOD.Home.Tests.UnitTests.Managers
{
    public class FeedingManagerScheduledFeedingTests
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        [Fact]
        public async Task EmptyScheduleSkipsFeedingAsync()
        {
            var now = DateTime.Parse("2020/04/01 10:00:00");

            _mocker
                .Setup<ISchedulingEngine, Task<DateTime?>>(e => e.CalculateNextSlotAsync(DefaultFeedingManager.ScheduleName, now.Date))
                .Returns(Task.FromResult<DateTime?>(null))
                .Verifiable();

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();

            var result = await manager.TryScheduledFeedAsync(now);

            _mocker.VerifyAll();

            Assert.True(result.Succeeded);
            Assert.Null(result.SlotUsed);
        }

        [Fact]
        public async Task ScheduleInTheFutureSkipsFeedingAsync()
        {
            var now = DateTime.Parse("2020/04/01 10:00:00");
            var future = DateTime.Parse("2020/04/01 12:00:00");

            _mocker
                .Setup<ISchedulingEngine, Task<DateTime?>>(e => e.CalculateNextSlotAsync(DefaultFeedingManager.ScheduleName, now.Date))
                .Returns(Task.FromResult<DateTime?>(future))
                .Verifiable();

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();

            var result = await manager.TryScheduledFeedAsync(now);

            _mocker.VerifyAll();

            Assert.True(result.Succeeded);
            Assert.Null(result.SlotUsed);
        }

        [Fact]
        public async Task ScheduleInThePastTriggersFeedingAsync()
        {
            var now = DateTime.Parse("2020/04/01 10:00:00");
            var future = DateTime.Parse("2020/04/01 09:50:00");

            _mocker
                .Setup<ISchedulingEngine, Task<DateTime?>>(e => e.CalculateNextSlotAsync(DefaultFeedingManager.ScheduleName, now.Date))
                .Returns(Task.FromResult<DateTime?>(future))
                .Verifiable();

            var sensorMock = _mocker.GetMock<IDigitalSensor>();
            var slot = _mocker.GetSlotWithSensor("dummy-42", sensorMock);
            slot.BypassSensor = true;

            var manager = _mocker.CreateInstance<DefaultFeedingManager>();
            manager.Slots = new[] { slot };

            var result = await manager.TryScheduledFeedAsync(now);

            _mocker.VerifyAll();

            Assert.True(result.Succeeded);
            Assert.NotNull(result.SlotUsed);
        }
    }
}
