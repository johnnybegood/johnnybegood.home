using System;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Models;
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

        /// <summary>
        /// Summary of the feeding
        /// </summary>
        [HttpGet()]
        public async Task<ActionResult<FeedingSummary>> Summary()
        {
            var summary = await _feedingManager.FeedingSummary(DateTime.UtcNow);

            return Ok(summary);
        }

        /// <summary>
        /// Next feeding
        /// </summary>
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
        public async Task<ActionResult<FeedResponse>> PostFeed()
        {
            var result = await _feedingManager.TryFeedAsync();
            var nextFeeding = await GetNextFeedingAsync();

            return new FeedResponse()
            {
                Succeeded = result.Succeeded,
                NextFeeding = nextFeeding
            };
        }
    }
}
