using System.Collections.Generic;

namespace JOHNNYbeGOOD.Home.Model
{
    /// <summary>
    /// Schedule for feeding
    /// </summary>
    public class FeedingSchedule
    {
        /// <summary>
        /// Temporary disable the automatic feeding schedule
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Slots of the schedule
        /// </summary>
        public ICollection<ScheduleSlot> Slots { get; set; }
    }
}
