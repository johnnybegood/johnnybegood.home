using System;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Model;
using JOHNNYbeGOOD.Home.Models;

namespace JOHNNYbeGOOD.Home.Resources
{
    public interface IScheduleResource
    {
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

        /// <summary>
        /// Retrieve the feeding
        /// </summary>
        Task<FeedingLogCollection> RetrieveFeedingLog();

        /// <summary>
        /// Log successfull feeding
        /// </summary>
        /// <param name="description">description of the feeding</param>
        /// <param name="dateTime">Date and time of the feeding</param>
        Task LogFeeding(string description, DateTime dateTime);

        /// <summary>
        /// Log failed feeding
        /// </summary>
        /// <param name="description">description of the feeding</param>
        /// <param name="dateTime">Date and time of the feeding</param>
        /// <param name="reason">Optional reason of failure</param>
        Task LogFailedFeeding(string description, DateTime dateTime, string reason = null);

        /// <summary>
        /// Last successfull feeding
        /// </summary>
        /// <param name="before">The date time to find the last successfull feeding relative to</param>
        Task<FeedingLog> LastFeeding(DateTime beforeDateTime);

        /// <summary>
        /// Last attempted feeding, includes failed feedings
        /// </summary>
        /// <param name="before">The date time to find the last attempted feeding relative to</param>
        Task<FeedingLog> LastFeedingAttempt(DateTime beforeDateTime);

        /// <summary>
        /// Clean log
        /// </summary>
        Task CleanUpLog();
    }
}
