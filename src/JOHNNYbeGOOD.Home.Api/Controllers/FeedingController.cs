using System;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Contracts;
using JOHNNYbeGOOD.Home.Api.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOHNNYbeGOOD.Home.Api.Controllers
{
    [Route("/api/feeding")]
    public class FeedingController : Controller, IFeedingService
    {
        private IFeedingManager _feedingManager;

        /// <summary>
        /// Default constructor for <see cref="FeedingController"/>
        /// </summary>
        /// <param name="feedingManager"></param>
        public FeedingController(IFeedingManager feedingManager)
        {
            _feedingManager = feedingManager;
        }

        /// <inheritdoc />
        [HttpGet()]
        public async Task<FeedingSummaryResponse> GetSummaryAsync()
        {
            var summary = await _feedingManager.FeedingSummary(DateTime.UtcNow);
            return new FeedingSummaryResponse
            {
                NextFeedingSlotName = summary.NextFeedingSlotName,
                NextFeedingTime = summary.NextFeedingTime,
                PreviousFeedingSlotName = summary.PreviousFeedingSlotName,
                PreviousFeedingTime = summary.PreviousFeedingTime
            };
        }

        /// <inheritdoc />
        [HttpGet("next")]
        public async Task<NextFeedingSlotResponse> GetNextFeedingAsync()
        {
            var nextSlot = _feedingManager.NextFeedingSlot();
            var nextTiming = await _feedingManager.NextFeedingTime(DateTime.UtcNow);

            return new NextFeedingSlotResponse
            {
                Slot = nextSlot?.Name,
                Timing = nextTiming
            };
        }

        /// <inheritdoc />
        [HttpPost("feed")]
        public async Task<FeedResponse> PostFeed()
        {
            var result = await _feedingManager.TryFeedAsync();
            var nextFeeding = await GetNextFeedingAsync();

            return new FeedResponse()
            {
                Succeeded = result.Succeeded,
                NextFeeding = nextFeeding
            };
        }

        /// <inheritdoc />
        [HttpGet("log")]
        public async Task<LogResponse[]> GetLog()
        {
            var log = await _feedingManager
                .RetrieveFeedingLog();

            var responses = log
                .Items
                .OrderByDescending(i => i.Timestamp)
                .Take(50)
                .Select(l => new LogResponse
                {
                    Id = l.Id,
                    DisplayId = l.Id.Substring(0, 8),
                    Result = l.Result.ToString(),
                    Cause = l.Cause,
                    Timestamp = l.Timestamp
                })
                .ToArray();

            return responses;
        }

        /// <inheritdoc />
        [HttpGet("schedule")]
        public async Task<ScheduleResponse> GetCurrentSchedule()
        {
            var schedule = await _feedingManager.RetrieveSchedule();
            var slots = schedule.Slots
                .Select(s => new ScheduleResponseSlot
                {
                    Hour = s.TimeOfDay.Hours,
                    Minutes = s.TimeOfDay.Minutes,
                    OnMonday = s.DayOfWeek.HasFlag(DayOfWeek.Monday),
                    OnTuesday = s.DayOfWeek.HasFlag(DayOfWeek.Tuesday),
                    OnWednesday = s.DayOfWeek.HasFlag(DayOfWeek.Wednesday),
                    OnThursday = s.DayOfWeek.HasFlag(DayOfWeek.Thursday),
                    OnFriday = s.DayOfWeek.HasFlag(DayOfWeek.Friday),
                    OnSaturday = s.DayOfWeek.HasFlag(DayOfWeek.Saturday),
                    OnSunday = s.DayOfWeek.HasFlag(DayOfWeek.Sunday)
                });

            return new ScheduleResponse { Slots = slots.ToList() };
        }
    }
}
