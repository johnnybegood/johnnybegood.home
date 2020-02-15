namespace JOHNNYbeGOOD.Home.Model.Devices
{
    public interface IDigitalSensor : IDevice
    {
        /// <summary>
        /// Read the current sensor value
        /// </summary>
        /// <returns>True if the sensor has a high value and false if the sensor has a low value</returns>
        bool Read();
    }
}
