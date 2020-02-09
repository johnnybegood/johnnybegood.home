using System;
using System.Linq;
using JOHNNYbeGOOD.Home.Model;

namespace JOHNNYbeGOOD.Home.Extensions
{
    /// <summary>
    /// Extensions for <see cref="DayOfWeek"/>
    /// </summary>
    public static class DayOfWeekExtensions
    {
        /// <summary>
        /// Convert <paramref name="dayOfWeek"/> to <see cref="DaysOfWeek"/>
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DaysOfWeek AsDaysOfWeek(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Friday => DaysOfWeek.Friday,
                DayOfWeek.Monday => DaysOfWeek.Monday,
                DayOfWeek.Saturday => DaysOfWeek.Saturday,
                DayOfWeek.Sunday => DaysOfWeek.Sunday,
                DayOfWeek.Thursday => DaysOfWeek.Thursday,
                DayOfWeek.Tuesday => DaysOfWeek.Tuesday,
                DayOfWeek.Wednesday => DaysOfWeek.Wednesday,
                _ => DaysOfWeek.None,
            };
        }

        /// <summary>
        /// Convert <paramref name="dayOfWeek"/> to <see cref="DaysOfWeek"/>
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DayOfWeek AsDayOfWeek(this DaysOfWeek dayOfWeek)
        {
            if (dayOfWeek.HasFlag(DaysOfWeek.Monday))
            {
                return DayOfWeek.Monday;
            }

            if (dayOfWeek.HasFlag(DaysOfWeek.Tuesday))
            {
                return DayOfWeek.Tuesday;
            }

            if (dayOfWeek.HasFlag(DaysOfWeek.Wednesday))
            {
                return DayOfWeek.Wednesday;
            }

            if (dayOfWeek.HasFlag(DaysOfWeek.Thursday))
            {
                return DayOfWeek.Thursday;
            }

            if (dayOfWeek.HasFlag(DaysOfWeek.Friday))
            {
                return DayOfWeek.Friday;
            }

            if (dayOfWeek.HasFlag(DaysOfWeek.Saturday))
            {
                return DayOfWeek.Saturday;
            }

            if (dayOfWeek.HasFlag(DaysOfWeek.Sunday))
            {
                return DayOfWeek.Sunday;
            }

            throw new ArgumentException("Invalid day of the week");
        }

        public static DayOfWeek Next(this DaysOfWeek from, DayOfWeek dayOfWeek)
        {
            var day = dayOfWeek.AsDaysOfWeek();

            var matching = Enum.GetValues(typeof(DayOfWeek))
                .OfType<DayOfWeek>()
                .Select(d => d.AsDaysOfWeek())
                .Where(d => from.HasFlag(d))
                .OrderBy(d => (int)d);

            var later = matching.Where(d => (int)d > (int)day);
            var next = later.Any() ? later.First() : matching.First();

            return next.AsDayOfWeek();
        }
    }
}
