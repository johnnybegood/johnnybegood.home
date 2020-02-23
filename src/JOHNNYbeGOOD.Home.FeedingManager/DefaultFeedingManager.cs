using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.Model;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        public DefaultFeedingManager(IOptionsSnapshot<FeedingManagerOptions> options, ILogger<DefaultFeedingManager> logger, ISchedulingEngine schedulingEngine, IThingsResource thingsResource)
        {
            _schedulingEngine = schedulingEngine;
            _logger = logger;
            _slots = new List<FeedingSlot>();

            foreach (var slotConfig in options.Value.FeedingSlots)
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
        public async Task<DateTime?> NextFeedingTime(DateTimeOffset afterDateTime)
        {
            var schedule = await RetrieveSchedule();
            var next = _schedulingEngine.CalculateNextSlot(schedule, afterDateTime);

            return next;
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
