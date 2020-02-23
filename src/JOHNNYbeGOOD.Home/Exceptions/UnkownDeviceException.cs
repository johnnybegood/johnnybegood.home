using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Exceptions
{
    [Serializable]
    public class UnkownDeviceException<T> : Exception where T : IDevice
    {
        public UnkownDeviceException() : base($"Unkown {typeof(T).Name} device")
        {
        }

        public UnkownDeviceException(string deviceId) : base($"Unkown {typeof(T).Name} device with id {deviceId}")
        {
        }

        public UnkownDeviceException(IEnumerable<string> deviceIds) : base($"Unkown {typeof(T).Name} device with ids {string.Join(", ", deviceIds)}")
        {
        }

        public UnkownDeviceException(string deviceId, Exception innerException) : base($"Unkown {typeof(T).Name} device with id {deviceId}", innerException)
        {
        }

        protected UnkownDeviceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}