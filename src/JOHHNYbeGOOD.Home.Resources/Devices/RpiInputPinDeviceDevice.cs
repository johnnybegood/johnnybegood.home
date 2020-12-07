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
                _controller.OpenPin(_pin, PinMode.InputPullDown);
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
            return IsConnected() && _controller.Read(_pin) == PinValue.High;
        }

        /// <inheritdoc />
        public DeviceStatus CurrentStatus()
        {
            if (_controller == null)
            {
                return DeviceStatus.Disconnected("GPIO controller not connected");
            }

            if (!_controller.IsPinOpen(_pin))
            {
                return DeviceStatus.Disconnected($"Pin {_pin} not open");
            }

            var value = _controller.Read(_pin);

            if (value == PinValue.High)
            {
                return DeviceStatus.Closed("High value on pin");
            }
            else if (value == PinValue.Low)
            {
                return DeviceStatus.Open("Low value on pin");
            }
            else
            {
                return DeviceStatus.Error($"Unkown value {value} on pin");
            }
        }
    }
}
