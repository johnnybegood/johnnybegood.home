using System;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOHNNYbeGOOD.Home.Api.Controllers
{
    [Route("/api/feeding")]
    public class FeedingController
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
            var result = _feedingManager.TryFeed();
            var nextFeeding = await GetNextFeedingAsync();

            return new FeedResponse()
            {
                Succeeded = result.Succeeded,
                NextFeeding = nextFeeding
            };
        }
    }
}
