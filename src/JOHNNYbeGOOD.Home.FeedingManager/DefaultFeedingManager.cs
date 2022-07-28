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
        public FeedingSlotDiagnostics[] GetDiagnostics()
        {
            return Slots
                .Select(s => new FeedingSlotDiagnostics { Id = s.Name, CanOpen = s.CanOpen() })
                .ToArray();
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

            var opened = await candidate.TryOpenFlapAsync();

            if (opened)
            {
                await _scheduleResource.LogFeeding(candidate.Name, DateTime.Now);
                _logger.LogInformation("Successfull feeding on slot {slot} at {time}", candidate.Name, DateTime.Now);
                return FeedingResult.Success(candidate.Name);
            }

            await _scheduleResource.LogFailedFeeding(candidate.Name, DateTime.Now, "Flap stuck");
            _logger.LogError("Failed feeding for slot {slot}", candidate.Name);
            return FeedingResult.Failed();
        }

        /// <inheritdoc />
        public async Task<DateTime?> NextFeedingTime(DateTime afterDateTime)
        {
            var offsetDate = await CalculateOffsetDateTimeAsync(afterDateTime);
            var next = await _schedulingEngine.CalculateNextSlotAsync(ScheduleName, offsetDate);

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
        public async Task<FeedingSummary> FeedingSummary(DateTime afterDateTime)
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
        public async Task<FeedingResult> TryScheduledFeedAsync(DateTime? now)
        {
            var nowDate = now ?? DateTime.Now;
            var nextFeeding = await NextFeedingTime(nowDate);

            if (nextFeeding.HasValue && nextFeeding.Value <= nowDate)
            {
                _logger.LogInformation("Trying feeding on schedule {next}", nextFeeding);
                return await TryFeedAsync();
            }

            _logger.LogDebug("Skipping feeding, next feeding {next}", nextFeeding);
            return FeedingResult.Skipped();
        }

        /// <summary>
        /// Calculate the date to use for the next feeding time calculations. Normally this is the last feeding attempt
        /// </summary>
        /// <param name="now">Current date time</param>
        /// <returns></returns>
        private async Task<DateTime> CalculateOffsetDateTimeAsync(DateTime now)
        {
            var lastFeeding = await _scheduleResource.LastFeedingAttempt(now);
            var scheduleUpdate = await _scheduleResource.RetrieveSchedule(ScheduleName);

            if (lastFeeding == null)
            {
                //If we never had a feeding, we use start of date. Any schedule for today will be use, even if it is an old one.
                return now.Date;
            }

            return scheduleUpdate.LastUpdated > lastFeeding.Timestamp
                ? scheduleUpdate.LastUpdated.Value
                : lastFeeding.Timestamp;
        }
    }
}
