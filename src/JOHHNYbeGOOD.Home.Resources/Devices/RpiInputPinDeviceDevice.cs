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
        private readonly PinValue _readPinValue;
        private GpioController _controller;

        /// <summary>
        /// Default constructor for <see cref="RpiInputPinDevice"/>
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="controller"></param>
        /// <param name="isNC">Flag indicating if device is NC (true) or NO (false)</param>
        public RpiInputPinDevice(int pin, bool isNC = false)
        {
            _pin = pin;
            _readPinValue = isNC ? PinValue.Low : PinValue.High;
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
            return IsConnected() && _controller.Read(_pin) == _readPinValue;
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

            if (value != PinValue.High && value != PinValue.Low)
            {
                return DeviceStatus.Error($"Unkown value {value} on pin");
            }
            else if (value == _readPinValue)
            {
                return DeviceStatus.Closed($"{value} value on pin");
            }
            else
            {
                return DeviceStatus.Open($"{value} value on pin");
            }
        }
    }
}
