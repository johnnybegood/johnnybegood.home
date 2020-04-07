using System;
namespace JOHNNYbeGOOD.Home.Api.Contracts.Models
{
    public class LogResponse
    {
        /// <summary>
        /// Unique identifier of the <see cref="LogResponse"/>
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Short version of <see cref="Id"/> for display use
        /// </summary>
        public string DisplayId { get; set; }

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
        public string Result { get; set; }

        /// <summary>
        /// Cause of unsuccessfull feeding
        /// </summary>
        public string Cause { get; set; }
    }
}
