using System;

namespace JOHNNYbeGOOD.Home.Models
{
    /// <summary>
    /// Single log of feeding
    /// </summary>
    public class FeedingLog
    {
        /// <summary>
        /// Unique identifier of the <see cref="FeedingLog"/>
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Date and time of the feeding
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Description of the feeding
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Result of the feeding (succesfull or otherwise)
        /// </summary>
        public FeedingLogResult Result { get; set; }

        /// <summary>
        /// Cause of unsuccessfull feeding
        /// </summary>
        public string Cause { get; set; }
    }
}