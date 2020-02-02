using System.Collections.Generic;

namespace JOHNNYbeGOOD.Home.Model
{
    /// <summary>
    /// Configuration of a group of devices
    /// </summary>
    public class DeviceGroupConfiguration
    {
        /// <summary>
        /// Name of the group
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Individual device configuration
        /// </summary>
        public ICollection<DeviceConfiguration> DeviceConfigration { get; set; }
    }
}
