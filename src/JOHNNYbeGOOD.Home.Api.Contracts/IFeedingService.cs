using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Contracts.Models;

namespace JOHNNYbeGOOD.Home.Api.Contracts
{
    /// <summary>
    /// Summary of the feeding
    /// </summary>
    public interface IFeedingService
    {
        /// <summary>
        /// Get the next scheduled feeding
        /// </summary>
        /// <returns></returns>
        Task<NextFeedingSlotResponse> GetNextFeedingAsync();

        /// <summary>
        /// Trigger a feeding with the next available slot
        /// </summary>
        /// <returns></returns>
        Task<FeedResponse> PostFeed();

        /// <summary>
        /// Get a summary of the feeder, including next feeding time and slot.
        /// </summary>
        /// <returns></returns>
        Task<FeedingSummaryResponse> GetSummaryAsync();

        /// <summary>
        /// Get the last items out the feeding log
        /// </summary>
        /// <returns></returns>
        Task<LogResponse[]> GetLog();

        /// <summary>
        /// Retrieve the current active schedule of the feeder
        /// </summary>
        /// <returns>The active schedule of the feeder</returns>
        Task<ScheduleDTO> GetCurrentSchedule();

        /// <summary>
        /// Put a new version of the schedule of the feeder
        /// </summary>
        Task PutCurrentSchedule(ScheduleDTO schedule);
    }
}