using System;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.Extensions;
using JOHNNYbeGOOD.Home.Model;
using JOHNNYbeGOOD.Home.Resources;

namespace JOHHNYbeGOOD.Home.Engines
{
    /// <summary>
    /// Default implementation of <see cref="ISchedulingEngine"/>
    /// </summary>
    public class SchedulingEngine : ISchedulingEngine
    {
        private IScheduleResource _scheduleResource;

        /// <summary>
        /// Default constructor for <see cref="SchedulingEngine"/>
        /// </summary>
        /// <param name="scheduleResource"></param>
        public SchedulingEngine(IScheduleResource scheduleResource)
        {
            _scheduleResource = scheduleResource;
        }
        /// <inheritdoc />
        public DateTime? CalculateNextSlot(Schedule schedule, DateTimeOffset afterDateTime)
        {
            if (schedule is null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            if (schedule.Disabled || schedule.Slots == null || !schedule.Slots.Any())
            {
                return null;
            }

            var nextRun = schedule.Slots
                .Select(s => CalculateNextRun(s, afterDateTime))
                .Where(dt => dt != null)
                .OrderBy(dt => dt.Value)
                .FirstOrDefault();

            return nextRun;
        }

        /// <inheritdoc />
        public DateTime? CalculateNextRun(ScheduleSlot slot, DateTimeOffset afterDateTime)
        {
            if (slot == null)
            {
                throw new ArgumentNullException(nameof(slot));
            }

            if (afterDateTime == null)
            {
                throw new ArgumentNullException(nameof(afterDateTime));
            }

            if (slot.DayOfWeek.Equals(DaysOfWeek.None))
            {
                return null;
            }

            if (slot.DayOfWeek.HasFlag(afterDateTime.DayOfWeek.AsDaysOfWeek()) && slot.TimeOfDay >= afterDateTime.TimeOfDay)
            {
                return afterDateTime.Date.Add(slot.TimeOfDay);
            }

            var next = slot.DayOfWeek.Next(afterDateTime.DayOfWeek);
            int daysUntil = ((int)next - (int)afterDateTime.DayOfWeek + 7) % 7;

            // If the next day is the current weekday, we add a week. Because we already now it is not today
            if (daysUntil == 0)
            {
                daysUntil = 7;
            }

            return afterDateTime.Date.AddDays(daysUntil).Add(slot.TimeOfDay);
        }

        /// <inheritdoc />
        public async Task<DateTime?> CalculateNextSlotAsync(string scheduleId, DateTimeOffset afterDateTime)
        {
            if (string.IsNullOrWhiteSpace(scheduleId))
            {
                throw new ArgumentException("message", nameof(scheduleId));
            }

            var schedule = await _scheduleResource.RetrieveSchedule(scheduleId);

            return CalculateNextSlot(schedule, afterDateTime);
        }
    }
}
