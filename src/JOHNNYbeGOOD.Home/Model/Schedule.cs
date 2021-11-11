using System;
using System.Collections.Generic;

namespace JOHNNYbeGOOD.Home.Model
{
    /// <summary>
    /// Schedule
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Temporary disable the automatic feeding schedule
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Last update of the schedule
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Slots of the schedule
        /// </summary>
        public ICollection<ScheduleSlot> Slots { get; set; } = new List<ScheduleSlot>();
    }
}
