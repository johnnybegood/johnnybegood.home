using System;

namespace JOHNNYbeGOOD.Home.Model
{
    /// <summary>
    /// Slot of a <see cref="FeedingSchedule"/>
    /// </summary>
    public class ScheduleSlot
    {
        /// <summary>
        /// Time of the day
        /// </summary>
        public TimeSpan TimeOfDay { get; set; }

        /// <summary>
        /// Day of the week
        /// </summary>
        public DaysOfWeek DayOfWeek { get; set; }
    }
}