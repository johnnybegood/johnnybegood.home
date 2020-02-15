using System;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.Extensions;
using JOHNNYbeGOOD.Home.Model;

namespace JOHHNYbeGOOD.Home.Engines
{
    /// <summary>
    /// Default implementation of <see cref="ISchedulingEngine"/>
    /// </summary>
    public class SchedulingEngine : ISchedulingEngine
    {
        /// <inheritdoc />
        public DateTime? CalculateNextSlot(Schedule schedule, DateTimeOffset afterDateTime)
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

        public Task<Schedule> RetrieveSchedule(string id)
        {
            throw new NotImplementedException();
        }

        public Task StoreSchedule(string id, Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
