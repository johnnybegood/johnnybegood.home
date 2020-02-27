using System;

namespace JOHNNYbeGOOD.Home.Model.Devices
{
    public class DeviceStatus
    {
        /// <summary>
        /// Device is connected
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// State code of the device
        /// </summary>
        public DeviceStateCode Code { get; set; }

        /// <summary>
        /// Description of the device status
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get <see cref="DeviceStatus"/> for disconnected device
        /// </summary>
        /// <param name="description">Extra description for the status</param>
        public static DeviceStatus Disconnected(string description)
        {
            return new DeviceStatus { IsConnected = false, Code = DeviceStateCode.Error, Description = description };
        }

        /// <summary>
        /// Get <see cref="DeviceStatus"/> for connected device in open state
        /// </summary>
        /// <param name="description">Extra description for the status</param>
        public static DeviceStatus Open(string description)
        {
            return new DeviceStatus { IsConnected = true, Code = DeviceStateCode.Open, Description = description };
        }

        /// <summary>
        /// Get <see cref="DeviceStatus"/> for connected device in closed state
        /// </summary>
        /// <param name="description">Extra description for the status</param>
        public static DeviceStatus Closed(string description)
        {
            return new DeviceStatus { IsConnected = true, Code = DeviceStateCode.Closed, Description = description };
        }

        /// <summary>
        /// Get <see cref="DeviceStatus"/> for connected device in transitioning state
        /// </summary>
        /// <param name="description">Extra description for the status</param>
        public static DeviceStatus Transitioning(string description)
        {
            return new DeviceStatus { IsConnected = true, Code = DeviceStateCode.Transitioning, Description = description };
        }

        //// <summary>
        /// Get <see cref="DeviceStatus"/> for device in error state
        /// </summary>
        /// <param name="description">Extra description for the status</param>
        /// <param name="isConnected">Connection status of device, defaults to true</param>
        public static DeviceStatus Error(string description, bool isConnected = true)
        {
            return new DeviceStatus { IsConnected = isConnected, Code = DeviceStateCode.Error, Description = description };
        }

        //// <summary>
        /// Get <see cref="DeviceStatus"/> for device in unkown state
        /// </summary>
        /// <param name="isConnected">Connection status of device, defaults to false</param>
        public static DeviceStatus Unkown(bool isConnected = false)
        {
            return new DeviceStatus { IsConnected = isConnected, Code = DeviceStateCode.Unkown };
        }
    }
}