using System;
using System.Threading.Tasks;
using JOHNNYbeGOOD.Home.Model;

namespace JOHNNYbeGOOD.Home.Resources.DummyDeviceResource
{
    /// <summary>
    /// Dummy implementation of <see cref="IDeviceResource"/> for faking devices when real devices are not accessable
    /// </summary>
    public class DummyDeviceResource : IDeviceResource
    {
        public Task ConfigureDeviceGroup(DeviceGroupConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public Task<Device> GetDevicesByGroup(string group)
        {
            throw new NotImplementedException();
        }
    }
}
