using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Contracts;
using JOHNNYbeGOOD.Home.Api.Contracts.Models;
using JOHNNYbeGOOD.Home.Model;
using Microsoft.AspNetCore.Mvc;

namespace JOHNNYbeGOOD.Home.Api.Controllers
{
    [Route("/api/feeding")]
    public class FeedingController : Controller
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
        public async Task<ScheduleDTO> GetCurrentSchedule()
        {
            var schedule = await _feedingManager.RetrieveSchedule();
            var slots = schedule.Slots
                .Select((s,i) => new ScheduleResponseSlot
                {
                    Id = i,
                    Hour = s.TimeOfDay.Hours,
                    Minutes = s.TimeOfDay.Minutes,
                    OnMonday = s.DayOfWeek.HasFlag(DaysOfWeek.Monday),
                    OnTuesday = s.DayOfWeek.HasFlag(DaysOfWeek.Tuesday),
                    OnWednesday = s.DayOfWeek.HasFlag(DaysOfWeek.Wednesday),
                    OnThursday = s.DayOfWeek.HasFlag(DaysOfWeek.Thursday),
                    OnFriday = s.DayOfWeek.HasFlag(DaysOfWeek.Friday),
                    OnSaturday = s.DayOfWeek.HasFlag(DaysOfWeek.Saturday),
                    OnSunday = s.DayOfWeek.HasFlag(DaysOfWeek.Sunday)
                });

            return new ScheduleDTO { Slots = slots.ToList() };
        }

        /// <inheritdoc />
        [HttpPut("schedule")]
        public async Task<IActionResult> PutCurrentSchedule([FromBody][Required] ScheduleDTO schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

                var newSchedule = new Schedule();
                newSchedule.Slots = schedule.Slots
                    .Select(s => new ScheduleSlot
                    {
                        TimeOfDay = new TimeSpan(s.Hour, s.Minutes, 0),
                        DayOfWeek = ToDayOfWeek(s)
                    })
                    .ToList();

            await _feedingManager.ScheduleFeeding(newSchedule);

            return Ok();
        }

        private DaysOfWeek ToDayOfWeek(ScheduleResponseSlot s)
        {
            var result = DaysOfWeek.None;

            if (s.OnMonday)
            {
                result |= DaysOfWeek.Monday;
            }

            if (s.OnTuesday)
            {
                result |= DaysOfWeek.Tuesday;
            }

            if (s.OnWednesday)
            {
                result |= DaysOfWeek.Wednesday;
            }

            if (s.OnThursday)
            {
                result |= DaysOfWeek.Thursday;
            }

            if (s.OnFriday)
            {
                result |= DaysOfWeek.Friday;
            }

            if (s.OnSaturday)
            {
                result |= DaysOfWeek.Saturday;
            }

            if (s.OnSunday)
            {
                result |= DaysOfWeek.Sunday;
            }

            return result;
        }
    }
}
