using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Resources.Devices
{
    /// <summary>
    /// Factory for creating <see cref="IDevice"/> instances
    /// </summary>
    public interface IDeviceFactory
    {
        /// <summary>
        /// Create device
        /// </summary>
        /// <typeparam name="TDevice">Type of device to create</typeparam>
        /// <param name="name">Unique name of device</param>
        /// <returns></returns>
        TDevice Create<TDevice>(string name) where TDevice : IDevice;
    }
}