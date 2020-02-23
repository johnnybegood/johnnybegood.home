using System.Device.Gpio;
using System.Device.I2c;

namespace JOHHNYbeGOOD.Home.Resources.Connectors
{
    /// <summary>
    /// Factory for connections to RaspberryPi onboard hardware
    /// </summary>
    public interface IRpiConnectionFactory
    {
        GpioController CreateGpio();

        /// <summary>
        /// Create <see cref="I2cDevice"/> for the given <paramref name="bus"/> and <paramref name="deviceAddress"/>
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="deviceAddress"></param>
        /// <returns></returns>
        I2cDevice CreateI2c(int bus, int deviceAddress);
    }
}