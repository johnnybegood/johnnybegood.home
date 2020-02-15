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
        FeedingResult TryFeed();

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
        /// Determine the next feeding slot
        /// </summary>
        /// <returns>The next feeding slot if any otherwise null</returns>
        IFeedingSlot NextFeedingSlot();
    }
}
