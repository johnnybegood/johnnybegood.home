using System.Device.Gpio;
using JOHHNYbeGOOD.Home.Resources.Connectors;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Resources.Devices
{
    /// <summary>
    /// Device for reading a GPIO pin as digital input
    /// </summary>
    public class RpiInputPinDevice : IDigitalSensor, IRpiDevice
    {
        private readonly int _pin;
        private GpioController _controller;

        /// <summary>
        /// Default constructor for <see cref="RpiInputPinDevice"/>
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="controller"></param>
        public RpiInputPinDevice(int pin)
        {
            _pin = pin;
        }

        /// <inheritdoc />
        public void Connect(IRpiConnectionFactory factory)
        {
            _controller = factory.CreateGpio();

            if (!_controller.IsPinOpen(_pin))
            {
                _controller.OpenPin(_pin, PinMode.Input);
            }
        }

        /// <inheritdoc />
        public bool IsConnected()
        {
            return _controller?.IsPinOpen(_pin) == true;
        }

        /// <inheritdoc />
        public bool Read()
        {
            return _controller.Read(_pin) == PinValue.High;
        }
    }
}
