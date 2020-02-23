using System;
using System.Device.I2c;
using System.Threading.Tasks;
using JOHHNYbeGOOD.Home.Resources.Connectors;
using JOHNNYbeGOOD.Home.Model.Devices;

namespace JOHHNYbeGOOD.Home.Resources.Devices
{
    public class DockerPiRelayChannelDevice : IGateDevice, IRpiDevice
    {
        private readonly byte[] _turnOnCommand;
        private readonly byte[] _turnOffCommand;
        private readonly int _bus;
        private readonly int _deviceAddress;
        private I2cDevice _i2c;

        public int Duration { get; set; } = 200;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="i2cDevice"></param>
        /// <param name="channel"></param>
        public DockerPiRelayChannelDevice(int bus, int deviceAddress, byte channel)
        {
            _bus = bus;
            _deviceAddress = deviceAddress;
            _turnOnCommand = new byte[]{ channel, 0xFF };
            _turnOffCommand = new byte[] { channel, 0x00 };
        }

        /// <inheritdoc />
        public bool IsConnected()
        {
            return _i2c != null;
        }

        /// <inheritdoc />
        public async Task OpenGateAsync()
        {
            if (!IsConnected())
            {
                throw new InvalidOperationException("Unable to open gate while disconnected");
            }

            if (Duration > 1000)
            {
                Duration = 1000;
            };

            _i2c.Write(_turnOnCommand);

            await Task.Delay(1000);

            _i2c.Write(_turnOffCommand);
        }

        /// <inheritdoc />
        public void Connect(IRpiConnectionFactory factory)
        {
            _i2c = factory.CreateI2c(_bus, _deviceAddress);
        }
    }
}
