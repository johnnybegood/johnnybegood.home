using System;
using System.Linq;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.Extensions;
using JOHNNYbeGOOD.Home.Model;

namespace JOHHNYbeGOOD.Home.Engines
{
    public class SchedulingEngine : ISchedulingEngine
    {
        /// <inheritdoc />
        public DateTime? CalculateNextSlot(FeedingSchedule schedule, DateTimeOffset from)
        {
            if (schedule is null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            if (schedule.Disabled || !schedule.Slots.Any())
            {
                return null;
            }

            var nextRun = schedule.Slots
                .Select(s => CalculateNextRun(s, from))
                .Where(dt => dt != null)
                .OrderBy(dt => dt.Value)
                .FirstOrDefault();

            return nextRun;
        }

        /// <summary>
        /// Calculate the next run for the given <paramref name="slot"/> relative to <paramref name="from"/>
        /// </summary>
        /// <param name="slot">The slot to use for the calculation</param>
        /// <param name="from">The date time to calculate the next run from</param>
        /// <returns>The next run date time or null if there is no next run</returns>
        public DateTime? CalculateNextRun(ScheduleSlot slot, DateTimeOffset from)
        {
            if (slot == null)
            {
                throw new ArgumentNullException(nameof(slot));
            }

            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (slot.DayOfWeek.Equals(DaysOfWeek.None))
            {
                return null;
            }

            if (slot.DayOfWeek.HasFlag(from.DayOfWeek.AsDaysOfWeek()) && slot.TimeOfDay >= from.TimeOfDay)
            {
                return from.Date.Add(slot.TimeOfDay);
            }

            var next = slot.DayOfWeek.Next(from.DayOfWeek);
            int daysUntil = ((int)next - (int)from.DayOfWeek + 7) % 7;

            // If the next day is the current weekday, we add a week. Because we already now it is not today
            if (daysUntil == 0)
            {
                daysUntil = 7;
            }

            return from.Date.AddDays(daysUntil).Add(slot.TimeOfDay);
        }
    }
}
