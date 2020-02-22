using System.Threading.Tasks;

namespace JOHNNYbeGOOD.Home.Model.Devices
{
    public interface IDevice
    {
        /// <summary>
        /// Check if the device is connected
        /// </summary>
        /// <returns></returns>
        bool IsConnected();
    }
}
