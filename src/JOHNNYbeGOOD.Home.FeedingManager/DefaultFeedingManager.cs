using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Engines;
using JOHNNYbeGOOD.Home.Model;
using JOHNNYbeGOOD.Home.Models;
using JOHNNYbeGOOD.Home.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JOHNNYbeGOOD.Home.FeedingManager
{
    public class DefaultFeedingManager : IFeedingManager
    {
        public const string ScheduleName = "feeding";
        private readonly ISchedulingEngine _schedulingEngine;
        private readonly ILogger<DefaultFeedingManager> _logger;
        private readonly IScheduleResource _scheduleResource;

        public IList<FeedingSlot> Slots { get; set; }

        /// <summary>
        /// Default constructor for <see cref="FeedingManager"/>
        /// </summary>
        /// <param name="schedulingEngine"></param>
        public DefaultFeedingManager(IOptions<FeedingManagerOptions> options, ILogger<DefaultFeedingManager> logger, ISchedulingEngine schedulingEngine, IThingsResource thingsResource, IScheduleResource scheduleResource)
        {
            _schedulingEngine = schedulingEngine;
            _logger = logger;
            _scheduleResource = scheduleResource;
            Slots = new List<FeedingSlot>();

            if (options.Value?.FeedingSlots?.Any() == true)
            {
                foreach (var slotConfig in options.Value.FeedingSlots)
                {
                    Slots.Add(new FeedingSlot(slotConfig, Slots, thingsResource));
                }
            }
        }

        /// <inheritdoc />
        public IFeedingSlot NextFeedingSlot()
        {
            return Slots.FirstOrDefault(s => s.CanOpen());
        }

        /// <inheritdoc />
        public async Task<FeedingResult> TryFeedAsync()
        {
            var candidate = (FeedingSlot) NextFeedingSlot();
            if (candidate == null)
            {
                await _scheduleResource.LogFailedFeeding("Skipped feeding", DateTime.Now, "No available gate for feeding");
                _logger.LogWarning("No candidate slot found for feeding");
                return FeedingResult.Failed();
            }

            _logger.LogDebug("Openening feeding slot {slot}", candidate.Name);

            if (candidate.TryOpenFlap())
            {
                await _scheduleResource.LogFeeding(candidate.Name, DateTime.Now);
                _logger.LogInformation("Successfull feeding on slot {slot} at {time}", candidate.Name, DateTime.Now);
                return FeedingResult.Success(candidate.Name);
            }

            await _scheduleResource.LogFailedFeeding(candidate.Name, DateTime.Now);
            _logger.LogError("Failed feeding for slot {slot}", candidate.Name);
            return FeedingResult.Failed();
        }

        /// <inheritdoc />
        public async Task<DateTime?> NextFeedingTime(DateTimeOffset afterDateTime)
        {
            var next = await _schedulingEngine.CalculateNextSlotAsync(ScheduleName, afterDateTime);

            return next;
        }

        /// <inheritdoc />
        public Task<Schedule> RetrieveSchedule()
        {
            return _scheduleResource.RetrieveSchedule(ScheduleName);
        }

        /// <inheritdoc />
        public Task ScheduleFeeding(Schedule schedule)
        {
            return _scheduleResource.StoreSchedule(ScheduleName, schedule);
        }

        /// <inheritdoc />
        public async Task<FeedingSummary> FeedingSummary(DateTimeOffset afterDateTime)
        {
            var lastFeeding = await _scheduleResource.LastFeeding(afterDateTime);
            var nextTime = await NextFeedingTime(afterDateTime);
            var nextSlot = NextFeedingSlot();

            return new FeedingSummary
            {
                NextFeedingTime = nextTime,
                NextFeedingSlotName = nextSlot?.Name,
                PreviousFeedingSlotName = lastFeeding?.Description,
                PreviousFeedingTime = lastFeeding?.Timestamp
            };
        }

        /// <inheritdoc />
        public Task<FeedingLogCollection> RetrieveFeedingLog()
        {
            return _scheduleResource.RetrieveFeedingLog();
        }

        /// <inheritdoc />
        public async Task<FeedingResult> TryScheduledFeedAsync(DateTimeOffset? now)
        {
            var nowOffset = now ?? DateTimeOffset.Now;
            var offsetDate = await CalculateOffsetDateTimeAsync(nowOffset);
            var nextFeeding = await NextFeedingTime(offsetDate);

            if (nextFeeding.HasValue && nextFeeding.Value <= nowOffset)
            {
                _logger.LogInformation("Trying feeding on schedule {next}", nextFeeding);
                return await TryFeedAsync();
            }

            _logger.LogDebug("Skipping feeding, next feeding {next}", nextFeeding);
            return FeedingResult.Skipped();
        }

        /// <summary>
        /// Calculate the offset date to use for the next feeding time calculations
        /// </summary>
        /// <param name="now">Current date time</param>
        /// <returns></returns>
        private async Task<DateTimeOffset> CalculateOffsetDateTimeAsync(DateTimeOffset now)
        {
            var lastFeeding = await _scheduleResource.LastFeeding(now);
            var scheduleUpdate = await _scheduleResource.RetrieveSchedule(ScheduleName);

            if (lastFeeding == null)
            {
                return now;
            }

            return scheduleUpdate.LastUpdated > lastFeeding.Timestamp
                ? scheduleUpdate.LastUpdated.Value
                : lastFeeding.Timestamp;
        }
    }
}
