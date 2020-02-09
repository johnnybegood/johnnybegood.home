using System;
using JOHHNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.Model;
using Xunit;

namespace JOHNNYbeGOOD.Home.Tests.UnitTests.Engines
{
    public class SchedulingEngineTests
    {

        [Fact]
        public void CalculatesRunForEverydaySlotAfterTime()
        {
            var from = new DateTime(2020, 1, 1, 10, 00, 00, DateTimeKind.Utc);
            var slot = new ScheduleSlot { DayOfWeek = DaysOfWeek.EveryDay, TimeOfDay = new TimeSpan(14, 00, 00) };

            var engine = new SchedulingEngine();
            var result = engine.CalculateNextRun(slot, from);

            Assert.Equal(new DateTime(2020, 1, 1, 14, 00, 00, DateTimeKind.Utc), result);
        }

        [Fact]
        public void CalculatesRunForEverydaySlotForNextDay()
        {
            var from = new DateTime(2020, 1, 1, 10, 00, 00, DateTimeKind.Utc);
            var slot = new ScheduleSlot { DayOfWeek = DaysOfWeek.EveryDay, TimeOfDay = new TimeSpan(9, 00, 00) };

            var engine = new SchedulingEngine();
            var result = engine.CalculateNextRun(slot, from);

            Assert.Equal(new DateTime(2020, 1, 2, 9, 00, 00, DateTimeKind.Utc), result);
        }

        [Fact]
        public void CalculatesRunForNextWeek()
        {
            var from = new DateTime(2020, 1, 1, 10, 00, 00, DateTimeKind.Utc);
            var slot = new ScheduleSlot { DayOfWeek = DaysOfWeek.Wednesday, TimeOfDay = new TimeSpan(9, 00, 00) };

            var engine = new SchedulingEngine();
            var result = engine.CalculateNextRun(slot, from);

            Assert.Equal(new DateTime(2020, 1, 8, 9, 00, 00, DateTimeKind.Utc), result);
        }

        [Fact]
        public void CalculatesRunForSunday()
        {
            var from = new DateTime(2020, 1, 1, 10, 00, 00, DateTimeKind.Utc);
            var slot = new ScheduleSlot { DayOfWeek = DaysOfWeek.Sunday, TimeOfDay = new TimeSpan(9, 00, 00) };

            var engine = new SchedulingEngine();
            var result = engine.CalculateNextRun(slot, from);

            Assert.Equal(new DateTime(2020, 1, 5, 9, 00, 00, DateTimeKind.Utc), result);
        }

        [Fact]
        public void CalculatesNoRun()
        {
            var from = new DateTime(2020, 1, 1, 10, 00, 00, DateTimeKind.Utc);
            var slot = new ScheduleSlot { DayOfWeek = DaysOfWeek.None, TimeOfDay = new TimeSpan(9, 00, 00) };

            var engine = new SchedulingEngine();
            var result = engine.CalculateNextRun(slot, from);

            Assert.Null(result);
        }
    }
}
