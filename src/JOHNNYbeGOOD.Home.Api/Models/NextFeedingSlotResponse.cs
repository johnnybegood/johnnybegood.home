using System;

namespace JOHNNYbeGOOD.Home.Api.Models
{
    public class NextFeedingSlotResponse
    {
        public string Slot { get; internal set; }
        public DateTime? Timing { get; internal set; }
    }
}