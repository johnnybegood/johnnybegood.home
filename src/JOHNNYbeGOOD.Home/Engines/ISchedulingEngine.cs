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
        /// Retrieve the active schedule for <paramref name="id"/>
        /// </summary>
        /// <param name="id">The ID of the schedule</param>
        /// <returns>The active schedule with the given <paramref name="id"/></returns>
        Task<Schedule> RetrieveSchedule(string id);

        /// <summary>
        /// Store a new schedule for <paramref name="id"/>
        /// </summary>
        /// <param name="id">The ID of the schedule</param>
        /// <param name="schedule">The new schedule to store</param>
        /// <returns></returns>
        Task StoreSchedule(string id, Schedule schedule);
    }
}
