using System.Threading.Tasks;

namespace JOHNNYbeGOOD.Home.Model.Devices
{
    public interface IDevice
    {
        /// <summary>
        /// Connect to the device
        /// </summary>
        /// <returns></returns>
        Task Connect();

        /// <summary>
        /// Check if the device is connected
        /// </summary>
        /// <returns></returns>
        bool IsConnected();
    }
}
