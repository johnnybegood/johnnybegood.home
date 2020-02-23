using System;
using System.Collections.Concurrent;
using System.Device.Gpio;
using System.Device.I2c;

namespace JOHHNYbeGOOD.Home.Resources.Connectors
{
    /// <summary>
    /// Factory for connections to RaspberryPi onboard hardware.
    /// </summary>
    public class RpiConnectionFactory : IDisposable, IRpiConnectionFactory
    {
        private readonly ConcurrentDictionary<string, I2cDevice> _i2cDevices;
        private readonly Lazy<GpioController> _gpioController;

        /// <summary>
        /// Default constructor for <see cref="RpiConnectionFactory"/>
        /// </summary>
        public RpiConnectionFactory()
        {
            _i2cDevices = new ConcurrentDictionary<string, I2cDevice>();
            _gpioController = new Lazy<GpioController>(() => new GpioController(PinNumberingScheme.Logical));
        }

        /// <inheritdoc />
        public I2cDevice CreateI2c(int bus, int deviceAddress)
        {
            var key = $"{bus}-{deviceAddress}";
            return _i2cDevices.GetOrAdd(key, key => I2cDevice.Create(new I2cConnectionSettings(bus, deviceAddress)));
        }

        /// <inheritdoc />
        public GpioController CreateGpio() => _gpioController.Value;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_gpioController.IsValueCreated)
                    {
                        _gpioController.Value.Dispose();
                    }

                    foreach (var i2c in _i2cDevices.Values)
                    {
                        i2c.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
