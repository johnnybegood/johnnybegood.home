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
        Task Feed();

        /// <summary>
        /// Schedule the feeding automatically
        /// </summary>
        /// <returns></returns>
        Task Schedule(FeedingSchedule schedule);

        /// <summary>
        /// Retrieve the current feeding schedule
        /// </summary>
        /// <returns></returns>
        Task<FeedingSchedule> RetrieveSchedule();
    }
}
