using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.Model;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JOHNNYbeGOOD.Home.FeedingManager
{
    public class DefaultFeedingManager : IFeedingManager
    {
        private const string ScheduleName = "feeding";
        private readonly ISchedulingEngine _schedulingEngine;
        private readonly ILogger<DefaultFeedingManager> _logger;
        private readonly IList<FeedingSlot> _slots;

        /// <summary>
        /// Default constructor for <see cref="FeedingManager"/>
        /// </summary>
        /// <param name="schedulingEngine"></param>
        public DefaultFeedingManager(IConfiguration configuration, ILogger<DefaultFeedingManager> logger, ISchedulingEngine schedulingEngine, IThingsResource thingsResource)
            : this(new List<FeedingSlot>(), logger, schedulingEngine)
        {
            var config = configuration.GetSection("feeder");

            foreach (var slotConfig in config.GetChildren())
            {
                _slots.Add(new FeedingSlot(slotConfig, _slots, thingsResource));
            }
        }

        /// <summary>
        /// Default constructor for <see cref="FeedingManager"/>
        /// </summary>
        /// <param name="schedulingEngine"></param>
        public DefaultFeedingManager(IEnumerable<FeedingSlot> feedingSlots, ILogger<DefaultFeedingManager> logger, ISchedulingEngine schedulingEngine)
        {
            _schedulingEngine = schedulingEngine;
            _logger = logger;
            _slots = feedingSlots.ToList();
        }

        /// <inheritdoc />
        public IFeedingSlot NextFeedingSlot()
        {
            return _slots.FirstOrDefault(s => s.CanOpen());
        }

        /// <inheritdoc />
        public FeedingResult TryFeed()
        {
            var candidate = (FeedingSlot) NextFeedingSlot();
            if (candidate == null)
            {
                _logger.LogWarning("No candidate slot found for feeding");
                return FeedingResult.Failed();
            }

            _logger.LogDebug("Openening feeding slot {slot}", candidate.Name);

            if (candidate.TryOpenGate())
            {
                return FeedingResult.Success(candidate.Name);
            }

            _logger.LogError("Failed feeding for slot {slot}", candidate.Name);
            return FeedingResult.Failed();
        }

        /// <inheritdoc />
        public Task<Schedule> RetrieveSchedule()
        {
            return _schedulingEngine.RetrieveSchedule(ScheduleName);
        }

        /// <inheritdoc />
        public Task ScheduleFeeding(Schedule schedule)
        {
            return _schedulingEngine.StoreSchedule(ScheduleName, schedule);
        }
    }
}
