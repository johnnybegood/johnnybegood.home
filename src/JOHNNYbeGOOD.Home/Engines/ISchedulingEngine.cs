using System;
using JOHNNYbeGOOD.Home.Model;

namespace JOHNNYbeGOOD.Home.Engines
{
    public interface ISchedulingEngine
    {
        DateTime? CalculateNextSlot(FeedingSchedule schedule, DateTimeOffset from);
    }
}
