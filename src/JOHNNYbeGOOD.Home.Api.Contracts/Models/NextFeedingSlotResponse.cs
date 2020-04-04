using System;

namespace JOHNNYbeGOOD.Home.Api.Contracts.Models
{
    public class NextFeedingSlotResponse
    {
        public string Slot { get; set; }
        public DateTime? Timing { get; set; }
    }
}