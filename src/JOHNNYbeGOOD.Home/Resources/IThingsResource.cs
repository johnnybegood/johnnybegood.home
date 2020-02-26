using System;
using System.Collections.Generic;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHNNYbeGOOD.Home.Resources
{
    public interface IThingsResource
    {
        /// <summary>
        /// Get known <typeparamref name="T"/> device
        /// </summary>
        /// <typeparam name="T">Type of devices to get</typeparam>
        /// <returns></returns>
        IReadOnlyCollection<T> GetDevices<T>() where T : IDevice;

        /// <summary>
        /// Get thing with <paramref name="id"/>
        /// </summary>
        /// <typeparam name="TDevice">Type of device</typeparam>
        /// <param name="id">Identifier of the device</param>
        /// <exception cref="ArgumentException">If the <paramref name="id"/> is unkown</exception>
        /// <returns>The device with the given identifier</returns>
        TDevice GetDevice<TDevice>(string id) where TDevice : IDevice;

        /// <summary>
        /// Full summary of known devices
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<DeviceSummary> FullDeviceSummary();
    }
}
