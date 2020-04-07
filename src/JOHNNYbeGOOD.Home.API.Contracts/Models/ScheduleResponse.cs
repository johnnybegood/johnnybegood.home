using System.Collections.Generic;

namespace JOHNNYbeGOOD.Home.Api.Contracts.Models
{
    public class ScheduleResponse
    {
        /// <summary>
        /// Slots of the schedule
        /// </summary>
        public IEnumerable<ScheduleResponseSlot> Slots { get; set; }
    }
}
