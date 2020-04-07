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

        /// <summary>
        /// Summary of the feeding
        /// </summary>
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

        /// <summary>
        /// Next feeding
        /// </summary>2
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

        /// <summary>
        /// Next feeding
        /// </summary>
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
    }
}
