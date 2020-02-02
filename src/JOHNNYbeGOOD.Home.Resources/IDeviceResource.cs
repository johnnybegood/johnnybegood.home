using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Model;

namespace JOHNNYbeGOOD.Home.Resources
{
    public interface IDeviceResource
    {
        Task ConfigureDeviceGroup(DeviceGroupConfiguration configuration);

        Task<Device> GetDevicesByGroup(string group);
    }
}
