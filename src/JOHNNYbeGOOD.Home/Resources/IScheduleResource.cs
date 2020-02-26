using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Model;

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
    }
}
