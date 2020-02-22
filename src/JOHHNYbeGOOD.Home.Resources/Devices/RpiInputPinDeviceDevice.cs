using System.Device.Gpio;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Resources.Devices
{
    /// <summary>
    /// Device for reading a GPIO pin as digital input
    /// </summary>
    public class RpiInputPinDevice : IDigitalSensor
    {
        private int _pin;
        private GpioController _controller;

        /// <summary>
        /// Default constructor for <see cref="RpiInputPinDevice"/>
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="controller"></param>
        public RpiInputPinDevice(int pin, GpioController controller)
        {
            _pin = pin;
            _controller = controller;
            controller.OpenPin(pin, PinMode.Input);
        }

        /// <inheritdoc />
        public bool IsConnected()
        {
            return _controller.IsPinOpen(_pin);
        }

        /// <inheritdoc />
        public bool Read()
        {
            return _controller.Read(_pin) == PinValue.High;
        }
    }
}
