using System;
namespace JOHNNYbeGOOD.Home.Api.Models
{
    public class StatusResponse
    {
        public string Device { get; internal set; }
        public string DeviceType { get; internal set; }
        public bool Connected { get; internal set; }
        public string State { get; internal set; }
        public string Description { get; internal set; }
    }
}
