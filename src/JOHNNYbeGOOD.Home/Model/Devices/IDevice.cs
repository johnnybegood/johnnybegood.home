namespace JOHNNYbeGOOD.Home.Model.Devices
{
    public interface IDevice
    {
        /// <summary>
        /// Check the current device status
        /// </summary>
        /// <returns></returns>
        DeviceStatus CurrentStatus(); 
    }
}
