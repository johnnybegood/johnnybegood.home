using System;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Model;

namespace JOHNNYbeGOOD.Home
{
    /// <summary>
    /// Manager for the feeding machine
    /// </summary>
    public interface IFeedingManager
    {
        /// <summary>
        /// Trigger feeding
        /// </summary>
        /// <returns></returns>
        Task<FeedingResult> TryFeedAsync();

        /// <summary>
        /// Schedule the feeding automatically
        /// </summary>
        /// <returns></returns>
        Task ScheduleFeeding(Schedule schedule);

        /// <summary>
        /// Retrieve the current feeding schedule
        /// </summary>
        /// <returns></returns>
        Task<Schedule> RetrieveSchedule();

        /// <summary>
        /// Get a summary of the current feeding status
        /// </summary>
        /// <param name="afterDateTime">The date time to calculate the summary on</param>
        /// <returns></returns>
        Task<FeedingSummary> FeedingSummary(DateTimeOffset afterDateTime);

        /// <summary>s
        /// Determine the next feeding slot
        /// </summary>
        /// <returns>The next feeding slot if any otherwise null</returns>
        IFeedingSlot NextFeedingSlot();

        /// <summary>
        /// Determine the next feeding time based on the actual <see cref="Schedule"/>
        /// </summary>
        /// <param name="afterDateTime">The date time to calculate the next feeding time from</param>
        /// <returns>The date time of the next feeding.</returns>
        Task<DateTime?> NextFeedingTime(DateTimeOffset afterDateTime);
    }
}
