using System;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Model;

namespace JOHNNYbeGOOD.Home.Engines
{
    /// <summary>
    /// Engine for scheduling
    /// </summary>
    public interface ISchedulingEngine
    {
        /// <summary>
        /// Calculate the next run of <paramref name="slot"/> after <paramref name="afterDateTime"/>
        /// </summary>
        /// <param name="slot">The slot to use for the calculation</param>
        /// <param name="from">The date time to calculate the next run from</param>
        /// <returns>The date time for the next run or null if there is no next run</returns>
        DateTime? CalculateNextRun(ScheduleSlot slot, DateTimeOffset afterDateTime);

        /// <summary>
        /// Calculate the next slot for the given <paramref name="schedule"/> after <paramref name="from"/>. 
        /// </summary>
        /// <param name="slot">The slot to use for the calculation</param>
        /// <param name="afterDateTime">The date time to calculate the next run from</param>
        /// <returns>The next run date time or null if there is no next run</returns>
        DateTime? CalculateNextSlot(Schedule schedule, DateTimeOffset afterDateTime);

        /// <summary>
        /// Calculate the next slot for the schedule after <paramref name="from"/>. 
        /// </summary>
        /// <param name="scheduleId">The name of the schedule to use</param>
        /// <param name="afterDateTime">The date time to calculate the next run from</param>
        /// <returns>The next run date time or null if there is no next run</returns>
        Task<DateTime?> CalculateNextSlotAsync(string scheduleId, DateTimeOffset afterDateTime);
    }
}
