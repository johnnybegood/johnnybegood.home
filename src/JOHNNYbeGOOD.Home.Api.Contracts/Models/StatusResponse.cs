using System;
namespace JOHNNYbeGOOD.Home.Api.Contracts.Models
{
    public class StatusResponse
    {
        public DeviceStatusResponse[] Devices { get; set; }

        public FeedingSlotStatusResponse[] FeedingSlot { get; set; }
    }
}
