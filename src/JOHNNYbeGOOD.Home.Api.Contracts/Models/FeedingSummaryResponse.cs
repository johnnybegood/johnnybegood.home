﻿using System;
namespace JOHNNYbeGOOD.Home.Api.Contracts.Models
{
    /// <summary>
    /// Summary of the current feeding status
    /// </summary>
    public class FeedingSummaryResponse
    {
        /// <summary>
        /// The next feeding time if any, otherwise null
        /// </summary>
        public DateTime? NextFeedingTime { get; set; }

        /// <summary>
        /// The next feeding slot if any, otherwise null
        /// </summary>
        public string NextFeedingSlotName { get; set; }

        /// <summary>
        /// The previous feeding time if any, otherwise null
        /// </summary>
        public DateTime? PreviousFeedingTime { get; set; }

        /// <summary>
        /// The previous feeding slot if any, otherwise null
        /// </summary>
        public string PreviousFeedingSlotName { get; set; }
    }
}
