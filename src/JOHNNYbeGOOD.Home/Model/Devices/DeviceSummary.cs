using System;

namespace JOHNNYbeGOOD.Home.Model.Devices
{
    /// <summary>
    /// Summary of a device
    /// </summary>
    public class DeviceSummary
    {
        public string Id { get; set; }

        public Type DeviceType { get; set; }

        public DeviceStatus Status { get; set; }
    }
}